using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AbstractClasses;
using UnityEngine;
using UnityEngine.Serialization;

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
    private float currentAngle = 0f;
    [SerializeField] private float orbitSpeed = 10f;
    [SerializeField] private float orbitRadius = 2f;
    [SerializeField] private float aimTime;
    private float aimTimeCache;
    private bool lockedOn;
    public GameObject reticle;
    private Vector3 reticleResetPos;
    private bool movingUp;
    
    [SerializeField] private float reticleTopLimit = 2;
    [SerializeField] private float reticleBottomLimit = -2;
    [SerializeField] private float reticleBounceSpeed = 10f;
    [SerializeField] private float shrinkSpeed;
    private float reticleTopResetPos;
    private float reticleBottomResetPos;
    
    #endregion
    
    public List<UniqueFollower> uniqueFollowers = new();
    private UniqueFollower activeFollower;
    [SerializeField] private GameObject uniqueFollowerPeg;
    void Start()
    {
        birbRB = GetComponent<Rigidbody2D>();
        targetBase.SetActive(false);
        cachedTargets = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlatformTrigger"));
        targetResetPos = targetBase.transform.localPosition;
        reticleResetPos = reticle.transform.localPosition;
        reticleTopResetPos = reticleTopLimit;
        reticleBottomResetPos = reticleBottomLimit;
        aimTimeCache = aimTime;
        followers.Clear();
        UpdateActiveFollower();
        for (int i = 0; i < numFollowers; i++)
        {
            AddFollower();
        }
    }

    private void Update()
    {
        HandleThrowing();
        HandleUseAbility();
        HandleCycleFollowers();
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            AddFollower();
        }
    }

    void FixedUpdate()
    {
        HandleLeadFollow();
        HandleFollowBird();
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
        if (Input.GetButton("Fire1") && !targetBase.activeSelf && followers.Count > 0)
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
            reticle.transform.localPosition = reticleResetPos;
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

            if (reticleTopLimit > 0 && reticleBottomLimit < 0)
            {
                reticleTopLimit -= shrinkSpeed * Time.deltaTime;
                reticleBottomLimit += shrinkSpeed * Time.deltaTime;
            }
        }
        Vector3 offset = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0) * orbitRadius;
        
        Quaternion targetRotation = Quaternion.Euler(0, 0, currentAngle);
        targetBase.transform.rotation = targetRotation;
        targetBase.transform.position = transform.position + offset;
    }

    private GameObject GetNearestTarget()
    {
        nearestTarget = null;
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

    #endregion

    #region Unique Follower Methods

    private void HandleUseAbility()
    {
        if (Input.GetButtonDown("Fire2") && activeFollower != null)
        {
            activeFollower.UseAbility();
            StartCoroutine(Cooldown(activeFollower.cooldown, activeFollower));
            uniqueFollowers.Remove(activeFollower);
            activeFollower = null;
            if (uniqueFollowers.Any())
            {
                UpdateActiveFollower();
            }
        }
    }

    private void HandleFollowBird()
    {
        if (uniqueFollowers.Any() && uniqueFollowers[^1].transform.position != uniqueFollowerPeg.transform.position)
        {
            foreach (var uniqueFollower in uniqueFollowers)
            {
                var targetPos = new Vector3(uniqueFollowerPeg.transform.position.x - uniqueFollowers.IndexOf(uniqueFollower), uniqueFollowerPeg.transform.position.y,0);
                //var targetPos = transform.position;
                uniqueFollower.transform.position = Vector2.Lerp(uniqueFollower.transform.position, targetPos, lerpTime);
            }
        }
    }

    private void HandleCycleFollowers()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CycleFollowerForward();
        }
    }
    private void UpdateActiveFollower()
    {
        if (uniqueFollowers.Any())
        {
            activeFollower = uniqueFollowers[0];
        }
    }
    

    private void CycleFollowerForward()
    {
        int lastIndex = uniqueFollowers.Count - 1;
        UniqueFollower lastItem = uniqueFollowers[lastIndex]; // Store the last item

        for (int i = lastIndex; i > 0; i--)
        {
            uniqueFollowers[i] = uniqueFollowers[i - 1]; // Shift elements forward by one
        }

        uniqueFollowers[0] = lastItem;
        UpdateActiveFollower();
    }

    private void SpawnUniqueFollowers()
    {
        foreach (var follower in uniqueFollowers)
        {
            Instantiate(follower);
        }
    }

    IEnumerator Cooldown(float waitTime, UniqueFollower followerToAdd)
    {
        yield return new WaitForSeconds(waitTime);
        followerToAdd.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        uniqueFollowers.Add(followerToAdd);
        UpdateActiveFollower();
        followerToAdd.DisableRbAndCollider();
    }
    #endregion
}