using UnityEngine;
using UnityEngine.Serialization;

public class PickupThrow : MonoBehaviour
{ // Start is called before the first frame update
    public GameObject targetreticle;

    public float throwForceUp;
    public float throwForceForward;
    [FormerlySerializedAs("rotationSpeed")] public float ringSpin;
    [FormerlySerializedAs("rotationSpeed")] public float targetMoveSpeed;

    private GameObject grabbedObject;
    private Rigidbody2D grabbedObjectRB;
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
                BirdFollowers.followers.RemoveAt(0);
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

    
}
