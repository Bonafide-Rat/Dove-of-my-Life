using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pickup_Throw : MonoBehaviour
{ // Start is called before the first frame update
    public GameObject targetreticle;

    public float throwForceUp;
    public float throwForceForward;
    [FormerlySerializedAs("rotationSpeed")] public float ringSpin;
    [FormerlySerializedAs("rotationSpeed")] public float targetMoveSpeed;

    private GameObject grabbedObject;
    private Rigidbody2D birbRB;
    private bool aiming;
    private Vector3 targetResetPos;
    void Start()
    {
        birbRB = GetComponent<Rigidbody2D>();
        targetreticle.SetActive(false);
        targetResetPos = targetreticle.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetButtonDown("Fire1") && !targetreticle.activeSelf && BirdFollowers.followers.Count > 0)
            {
                grabbedObject = BirdFollowers.followers[0];
                targetreticle.SetActive(true);
                aiming = true;
            }
            
            else if (Input.GetButtonDown("Fire1") && targetreticle.activeSelf)
            {
                Vector2 throwDirection = targetreticle.transform.localPosition - transform.position;
                targetreticle.SetActive(false);
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                grabbedObject.GetComponent<Collider2D>().isTrigger = false;
                grabbedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(throwDirection.x * throwForceForward, throwDirection.y + throwForceUp);
                grabbedObject.GetComponent<Rigidbody2D>().angularVelocity += ringSpin;
                aiming = false;
                targetreticle.transform.localPosition = targetResetPos;
                BirdFollowers.followers.RemoveAt(0);
                grabbedObject = null;
            }
            
        if (aiming)
        {
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
        
    }
    
    private void Easythrow()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
            grabbedObject.GetComponent<Collider2D>().isTrigger = false;
            grabbedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(birbRB.velocity.x * throwForceForward, birbRB.velocity.y + throwForceUp);
            grabbedObject.GetComponent<Rigidbody2D>().angularVelocity += ringSpin;
            grabbedObject = null;
        }
    }

    
}
