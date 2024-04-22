using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    #region Follower Options

    [SerializeField] private int numFollowers;
    [SerializeField] private GameObject baseFollower;
    public float lerpTime = 0.5f;
    public static List<GameObject> followers = new();

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
    


    void Start()
    {
        birbRB = GetComponent<Rigidbody2D>();
        targetreticle.SetActive(false);
        targetResetPos = targetreticle.transform.localPosition;
        followers.Clear();
        for (int i = 0; i < numFollowers; i++)
        {
            AddFollower();
        }
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
        GameObject follower = Instantiate(baseFollower, transform.position, Quaternion.identity);
        SpriteRenderer spriteRenderer = follower.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "Midground"; // Ensuring the follower is visible
        } else {
            Debug.Log("The follower prefab does not have a Sprite Renderer component.");
        }

        followers.Add(follower);
    }

    private void HandleLeadFollow()
    {
        if (followers.Count > 0 && followers[^1].transform.position != transform.position)
        {
            followers[0].transform.position =
                Vector2.Lerp(followers[0].transform.position, transform.position, lerpTime);
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
}