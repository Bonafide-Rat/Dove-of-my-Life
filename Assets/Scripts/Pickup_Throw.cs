using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pickup_Throw : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform pickupPoint;
    public Transform castPoint;
    public GameObject targetreticle;
    public float pickupRange;

    public float throwForceUp;
    public float throwForceForward;
    [FormerlySerializedAs("rotationSpeed")] public float ringSpin;
    [FormerlySerializedAs("rotationSpeed")] public float targetMoveSpeed;

    public bool easyThrowMode = true;

    private GameObject grabbedObject;
    private Rigidbody2D birbRB;

    private int layerIndex;
    private bool aiming;
    private Vector3 targetResetPos;
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("Object");
        birbRB = GetComponent<Rigidbody2D>();
        targetreticle.SetActive(false);
        targetResetPos = targetreticle.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(castPoint.position, transform.right, pickupRange);

        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer.Equals(layerIndex))
        {
            if (Input.GetButtonDown("Fire1") && grabbedObject == null)
            {
                grabbedObject = hitInfo.collider.gameObject;
                grabbedObject.GetComponent<Collider2D>().isTrigger = true;
                grabbedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                grabbedObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                grabbedObject.transform.SetParent(transform);
                grabbedObject.transform.position = pickupPoint.position;
            }
            
        }
        
        else if (grabbedObject !=null && easyThrowMode)
        {
            Easythrow();
        }
        
        else if (grabbedObject !=null && !easyThrowMode)
        {
            if (Input.GetButtonDown("Fire1") && !targetreticle.activeSelf )
            {
                targetreticle.SetActive(true);
                aiming = true;
                
            }
            
            else if (Input.GetButtonDown("Fire1") && targetreticle.activeSelf)
            {
                Vector2 throwDirection = targetreticle.transform.position - transform.position;
                targetreticle.SetActive(false);
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                grabbedObject.GetComponent<Collider2D>().isTrigger = false;
                grabbedObject.transform.SetParent(null);
                grabbedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(throwDirection.x * throwForceForward, throwDirection.y + throwForceUp);
                grabbedObject.GetComponent<Rigidbody2D>().angularVelocity += ringSpin;
                aiming = false;
                targetreticle.transform.localPosition = targetResetPos;
                grabbedObject = null;
            }
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
        //Debug.DrawRay(castPoint.position, transform.right, Color.red, pickupRange);
    }
    
    private void Easythrow()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
            grabbedObject.GetComponent<Collider2D>().isTrigger = false;
            grabbedObject.transform.SetParent(null);
            grabbedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(birbRB.velocity.x * throwForceForward, birbRB.velocity.y + throwForceUp);
            grabbedObject.GetComponent<Rigidbody2D>().angularVelocity += ringSpin;
            grabbedObject = null;
        }
    }

    
}
