using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Throw : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform pickupPoint;
    public Transform castPoint;
    public GameObject targetreticle;
    public float pickupRange;

    public float throwForceUp;
    public float throwForceForward;
    public float rotationSpeed;

    public bool easyThrowMode = true;

    private GameObject grabbedObject;
    private Rigidbody2D birbRB;

    private int layerIndex;
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("Object");
        birbRB = GetComponent<Rigidbody2D>();
        targetreticle.SetActive(false);
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
            }
            
            else if (Input.GetButtonDown("Fire1") && targetreticle.activeSelf)
            {
                Vector2 throwDirection = targetreticle.transform.position - transform.position;
                targetreticle.SetActive(false);
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                grabbedObject.GetComponent<Collider2D>().isTrigger = false;
                grabbedObject.transform.SetParent(null);
                grabbedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(throwDirection.x * throwForceForward, throwDirection.y + throwForceUp);
                grabbedObject.GetComponent<Rigidbody2D>().angularVelocity += rotationSpeed;
                grabbedObject = null;
            }
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
            grabbedObject.GetComponent<Rigidbody2D>().angularVelocity += rotationSpeed;
            grabbedObject = null;
        }
    }

    
}
