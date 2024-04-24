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
    #endregion
    
    public List<UniqueFollower> uniqueFollowers = new();
    private UniqueFollower activeFollower;
    [SerializeField] private GameObject uniqueFollowerPeg;
    private GameObject followerOne;
    private GameObject followerTwo;
    private GameObject followerThree;
    private GameObject followerFour;

    void Start()
    {
        birbRB = GetComponent<Rigidbody2D>();
        targetreticle.SetActive(false);
        targetResetPos = targetreticle.transform.localPosition;
        followers.Clear();
        AssignFollowerObjects();
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
        if (Input.GetButtonDown("Fire1") && !targetreticle.activeSelf && followers.Count > 0)
        {
            grabbedObject = followers[0];
            grabbedObjectRB = grabbedObject.GetComponent<Rigidbody2D>();
            targetreticle.SetActive(true);
            aiming = true;
        }
            
        else if (Input.GetButtonDown("Fire1") && targetreticle.activeSelf)
        {
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

        if (!aiming) return;
        if (targetreticle.transform.localPosition.y >= 1)
        {
            targetMoveSpeed *= -1;
        }
            
        if (targetreticle.transform.localPosition.y <= -0.65)
        {
                
            targetMoveSpeed *= -1;
        }
        targetreticle.transform.RotateAround(transform.position,Vector3.forward,targetMoveSpeed * Time.deltaTime);
    }
    
    private void Easythrow()
    {
        if (!Input.GetButtonDown("Fire1")) return;
        grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
        grabbedObject.GetComponent<Collider2D>().isTrigger = false;
        var velocity = birbRB.velocity;
        grabbedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * throwForceForward, velocity.y + throwForceUp);
        grabbedObject.GetComponent<Rigidbody2D>().angularVelocity += ringSpin;
        grabbedObject = null;
    }

    #endregion

    #region Unique Follower Methods

    private void HandleUseAbility()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (activeFollower.throwable)
            {
                activeFollower.Throw();
            }
            activeFollower.UseAbility();
            StartCoroutine(Cooldown(activeFollower.cooldown, activeFollower));
            uniqueFollowers.Remove(activeFollower);
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

    private void AssignFollowerObjects()
    {
        foreach (var follower in uniqueFollowers)
        {
            switch (follower.followerName)
            {
                case "test1":
                    followerOne = follower.gameObject;
                    break;
                case "test2":
                    followerTwo = follower.gameObject;
                    break;
            }
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
        followerToAdd.DisableRbAndCollider();
        uniqueFollowers.Add(followerToAdd);
        UpdateActiveFollower();
    }
    #endregion
}