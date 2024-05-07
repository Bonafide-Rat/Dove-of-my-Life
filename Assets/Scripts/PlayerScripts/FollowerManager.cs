using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AbstractClasses;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class FollowerManager : MonoBehaviour
{
    #region Follower Options

    [SerializeField] private int numFollowers;
    [SerializeField] private GameObject baseFollower;
    public float lerpTime = 0.5f;
    public static List<GameObject> followers = new();
    public static int followerCount;
    [SerializeField]private GameObject followerPeg;

    #endregion

    #region Throw Options

    public float throwForceUp;
    public float throwForceForward;
    public float ringSpin;
    private GameObject grabbedObject;
    private Rigidbody2D grabbedObjectRB;
    private Rigidbody2D birbRB;

    #endregion

    #region Target Reticle
    private bool aiming;
    private Vector3 targetResetPos;
    public GameObject targetBase;
    private List<GameObject> cachedTargets = new();
    private GameObject nearestTarget;
    [SerializeField] private float distanceLimit;
    private float currentAngle;
    [SerializeField] private float targeterOrbitRadius = 2f;
    [SerializeField] private float aimTime;
    private float aimTimeCache;
    private bool lockedOn;
    public GameObject reticle;
    private Vector3 reticleResetPos;
    private bool movingUp;
    private bool lastFacingRight = true;
    
    [SerializeField] private float reticleTopLimit;
    [SerializeField] private float reticleBottomLimit;
    [SerializeField] private float reticleBounceSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private GameObject topBumper;
    [SerializeField] private GameObject bottomBumper;
    private float reticleTopResetPos;
    private float reticleBottomResetPos;

    private Quaternion initialTopRotation;
    private Quaternion initialBottomRotation;
    private Vector3 initialTopPosition;
    private Vector3 initialBottomPosition;
    
    #endregion
    void Start()
    {
        birbRB = GetComponent<Rigidbody2D>();
        targetBase.SetActive(false);
        cachedTargets = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlatformTrigger"));
        targetResetPos = targetBase.transform.localPosition;
        reticleResetPos = reticle.transform.localPosition;
        reticleTopResetPos = reticleTopLimit;
        reticleBottomResetPos = reticleBottomLimit;
        initialTopRotation = topBumper.transform.rotation;
        initialBottomRotation = bottomBumper.transform.rotation;
        initialTopPosition = topBumper.transform.localPosition;
        initialBottomPosition = bottomBumper.transform.localPosition;
        aimTimeCache = aimTime;
        followers.Clear();
        for (int i = 0; i < numFollowers; i++)
        {
            AddFollower();
        }

        GameManagerScript.OnRespawn += ResetFollowers;
    }

    private void Update()
    {
        HandleThrowing();
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            AddFollower();
        }
    }

    void FixedUpdate()
    {
        HandleLeadFollow();
    }


    #region Follower Methods

    public void AddFollower()
    {
        followers.Add(Instantiate(baseFollower, transform.position, Quaternion.identity));
        followerCount = followers.Count;
    }

    private void HandleLeadFollow()
    {
        if (followers.Count > 0 && followers[^1].transform.position != transform.position)
        {
            followers[0].transform.position =
                Vector2.Lerp(followers[0].transform.position, followerPeg.transform.position, lerpTime);
        }
    }

    #endregion

    #region Throw Methods

    private void HandleThrowing()
    {
        if (Input.GetButton("Fire1") && !targetBase.activeSelf && followers.Count > 0 && GetNearestTarget() is not null)
        {
            grabbedObject = followers[0];
            grabbedObjectRB = grabbedObject.GetComponent<Rigidbody2D>();
            targetBase.SetActive(true);
            aiming = true;
        }
            
        else if (Input.GetButtonUp("Fire1") && targetBase.activeSelf)
        {
            grabbedObject.transform.position = transform.position;
            Vector2 throwDirection = reticle.transform.position - transform.position;
            targetBase.SetActive(false);
            grabbedObjectRB.isKinematic = false;
            //grabbedObjectRB.enabled = true; - Not sure what this is meant to do
            grabbedObjectRB.velocity = throwDirection * throwForceForward;
            grabbedObjectRB.angularVelocity += ringSpin;
            grabbedObject.GetComponent<Collider2D>().enabled = true;
            aiming = false;
            targetBase.transform.localPosition = targetResetPos;
            reticle.transform.localPosition = new Vector3(reticle.transform.localPosition.x,Random.Range(reticleBottomLimit,reticleTopLimit),0);
            reticleTopLimit = reticleTopResetPos;
            reticleBottomLimit = reticleBottomResetPos;
            followers.RemoveAt(0);
            followerCount = followers.Count;
            grabbedObject = null;
            lockedOn = false;
            aimTime = aimTimeCache;
        }
        HandleAim();
    }
    
    private void AdjustTargetScale()
    {
        // Explicitly set scale based on the facing direction
        if (PlayerController.isFacingRight)
        {
            targetBase.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); // Reset to normal scale
        }
        else
        {
            targetBase.transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f); // Flip horizontally
        }
    }


    private void HandleAim()
    {
        if (!aiming) return;
        aimTime -= Time.deltaTime;
        if (aimTime <= 0)
        {
            lockedOn = true;
        }
        Vector3 targetVector = (GetNearestTarget().transform.position - transform.position).normalized;
        Debug.DrawLine(transform.position, GetNearestTarget().transform.position, Color.red);
        float angleToTarget = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;
        
        if (PlayerController.isFacingRight != lastFacingRight)
        {
            AdjustTargetScale();
            lastFacingRight = PlayerController.isFacingRight; // Update the last known direction
        }
        currentAngle = angleToTarget;
        if (lockedOn)
        {
            reticle.transform.localPosition = reticleResetPos;
        }
        else
        {
            Vector3 reticlePos = reticle.transform.localPosition;
            // Move the reticle based on the current direction
            if (movingUp)
            {
                reticlePos.y += reticleBounceSpeed * Time.deltaTime;

                // If the reticle reaches the upper bound, change direction
                if (reticlePos.y >= reticleTopLimit)
                {
                    movingUp = false;
                }
            }
            else
            {
                reticlePos.y -= reticleBounceSpeed * Time.deltaTime;

                // If the reticle reaches the lower bound, change direction
                if (reticlePos.y <= reticleBottomLimit)
                {
                    movingUp = true;
                }
            }
            
            reticle.transform.localPosition = reticlePos;
            HandleShrink(topBumper,bottomBumper);
            
        }
        Vector3 offset = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0) * targeterOrbitRadius;
        Quaternion targetRotation = Quaternion.Euler(0, 0, currentAngle);
        targetBase.transform.rotation = targetRotation;
        targetBase.transform.position = transform.position + offset;
    }

    private void HandleShrink(GameObject topBumper, GameObject bottomBumper)
    {

        if (reticleTopLimit > 0 && reticleBottomLimit < 0)
        {
            float shrinkProgress = Mathf.InverseLerp(aimTimeCache, 0f, aimTime);
            reticleTopLimit = Mathf.Lerp(reticleTopResetPos, 0f, shrinkProgress);
            reticleBottomLimit = Mathf.Lerp(reticleBottomResetPos, 0f, shrinkProgress);
            Quaternion targetRotation = Quaternion.identity; // Zero degrees rotation
            Vector3 targetPosition = new Vector3(initialTopPosition.x, 0, initialTopPosition.z);
            topBumper.transform.localRotation = Quaternion.Lerp(
                initialTopRotation,
                targetRotation,
                shrinkProgress);

            bottomBumper.transform.localRotation = Quaternion.Lerp(
                initialBottomRotation,
                targetRotation,
                shrinkProgress);

            topBumper.transform.localPosition = Vector3.Lerp(initialTopPosition, targetPosition, shrinkProgress);
            bottomBumper.transform.localPosition = Vector3.Lerp(initialBottomPosition, targetPosition, shrinkProgress);
        }
    }

    private GameObject GetNearestTarget()
    {
        nearestTarget = null;
        UpdateCachedTargets();
        float shortestDistance = float.MaxValue;
        Vector3 currentPosition = transform.position;

        foreach (var target in cachedTargets)
        {
            float distance = Vector3.Distance(currentPosition, target.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTarget = target;
            }
        }
        return nearestTarget;
    }

    private void UpdateCachedTargets()
    {
        for (int i = 0; i < cachedTargets.Count -1; i++)
        {
            if (!cachedTargets[i])
            {
                cachedTargets.RemoveAt(i);
            }
        }
    }

    private void ResetFollowers()
    {
        foreach (var follower in followers)
        {
            Destroy(follower);
        }
        followers.Clear();
        followerCount = 0;
    }

    #endregion
}