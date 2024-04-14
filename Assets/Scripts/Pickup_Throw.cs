using UnityEngine;
using UnityEngine.Serialization;

public class PickupThrow : MonoBehaviour
{ // Start is called before the first frame update
    public GameObject targetreticle;

    public float throwForceUp;
    public float throwForceForward;
    [FormerlySerializedAs("rotationSpeed")] public float ringSpin;
    [FormerlySerializedAs("rotationSpeed")] public float targetMoveSpeed;

    private GameObject followerToBeThrown;
    private Rigidbody2D followerToBeThrownRB;
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
                followerToBeThrown = BirdFollowers.followers[0];
                followerToBeThrownRB = followerToBeThrown.GetComponent<Rigidbody2D>();
                targetreticle.SetActive(true);
                aiming = true;
            }
            
            else if (Input.GetButtonDown("Fire1") && targetreticle.activeSelf)
            {
                Vector2 throwDirection = targetreticle.transform.position - transform.position;
                targetreticle.SetActive(false);
                followerToBeThrownRB.isKinematic = false;
                //grabbedObjectRB.enabled = true; - Not sure what this is meant to do
                followerToBeThrown.GetComponent<Collider2D>().enabled = true;
                followerToBeThrownRB.velocity = throwDirection * throwForceForward;
                followerToBeThrownRB.angularVelocity += ringSpin;
                aiming = false;
                targetreticle.transform.localPosition = targetResetPos;
                BirdFollowers.followers.RemoveAt(0);
                followerToBeThrown = null;
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
        followerToBeThrown.GetComponent<Rigidbody2D>().isKinematic = false;
        followerToBeThrown.GetComponent<Collider2D>().isTrigger = false;
        followerToBeThrown.GetComponent<Rigidbody2D>().velocity = new Vector2(birbRB.velocity.x * throwForceForward, birbRB.velocity.y + throwForceUp);
        followerToBeThrown.GetComponent<Rigidbody2D>().angularVelocity += ringSpin;
        followerToBeThrown = null;
    }

    
}
