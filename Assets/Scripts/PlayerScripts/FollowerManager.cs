using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AbstractClasses;
using UnityEngine;

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
    public GameObject targetreticle;
    public float targetMoveSpeed;
    [SerializeField] private float bottomTargetBuffer;
    [SerializeField] private float topTargetBuffer;
    [SerializeField] private float convergenceRate;
    private List<GameObject> cachedTargets = new();
    private GameObject nearestTarget;
    #endregion
    
    public List<UniqueFollower> uniqueFollowers = new();
    private UniqueFollower activeFollower;
    [SerializeField] private GameObject uniqueFollowerPeg;
    
    public float radius = 5.0f; // Radius of the circular path
    public float rotationSpeed = 90.0f;

    void Start()
    {
        birbRB = GetComponent<Rigidbody2D>();
        targetreticle.SetActive(false);
        cachedTargets = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlatformTrigger"));
        targetResetPos = targetreticle.transform.localPosition;
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
        if (Input.GetButton("Fire1") && !targetreticle.activeSelf && followers.Count > 0)
        {
            grabbedObject = followers[0];
            grabbedObjectRB = grabbedObject.GetComponent<Rigidbody2D>();
            targetreticle.SetActive(true);
            aiming = true;
        }
            
        else if (Input.GetButtonUp("Fire1") && targetreticle.activeSelf)
        {
            grabbedObject.transform.position = transform.position;
            Vector2 throwDirection = targetreticle.transform.position - transform.position;
            targetreticle.SetActive(false);
            grabbedObjectRB.isKinematic = false;
            //grabbedObjectRB.enabled = true; - Not sure what this is meant to do
            grabbedObjectRB.velocity = throwDirection * throwForceForward;
            grabbedObjectRB.angularVelocity += ringSpin;
            grabbedObject.GetComponent<Collider2D>().enabled = true;
            aiming = false;
            targetreticle.transform.localPosition = targetResetPos;
            followers.RemoveAt(0);
            followerCount = followers.Count;
            grabbedObject = null;
        }
        HandleAim();
    }


    private void HandleAim()
    {
        if (!aiming) return;
        Vector3 targetVector = GetNearestTarget().transform.position - transform.position;
        // Debug.DrawLine(transform.position,GetNearestTarget().transform.position,Color.red);
        //
        // targetreticle.transform.RotateAround(transform.position,Vector3.forward,targetMoveSpeed * Time.deltaTime);
        //
        // float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;
        //
        // // Ensure the child aims at the target
        // targetreticle.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        
        // Get the vector from parent to target

        // Calculate the closest point on the circle to the target
        Vector3 closestPointOnCircle = transform.position + targetVector.normalized * radius;

        // Calculate the current position on the circle
        Vector3 currentPosition = targetreticle.transform.position - transform.position;

        // Calculate the angle between the current position and the closest point on the circle
        float angle = Mathf.Atan2(closestPointOnCircle.y, closestPointOnCircle.x) - Mathf.Atan2(currentPosition.y, currentPosition.x);

        // Ensure the angle is within 0 to 360 degrees
        if (angle < 0) angle += 2 * Mathf.PI;

        // Rotate the child to the closest point on the circle
        float step = rotationSpeed * Time.deltaTime;
        float newAngle = Mathf.MoveTowardsAngle(Mathf.Rad2Deg * Mathf.Atan2(currentPosition.y, currentPosition.x), Mathf.Rad2Deg * Mathf.Atan2(closestPointOnCircle.y, closestPointOnCircle.x), step);

        // Set the new position of the child on the circular path
        float radians = Mathf.Deg2Rad * newAngle;
        targetreticle.transform.position = transform.position + new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * radius;
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