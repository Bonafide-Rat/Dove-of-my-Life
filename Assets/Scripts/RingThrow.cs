using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRingInteraction : MonoBehaviour
{
    public GameObject targetReticle;
    public float throwForceUp;
    public float throwForceForward;
    public float ringSpin;
    public float targetMoveSpeed;

    private Rigidbody2D ringRB;
    private bool aiming;
    private Vector3 targetResetPos;
    private Vector3 initialRingPosition;
    private Quaternion initialRingRotation;

    void Start()
    {
        ringRB = ring.GetComponent<Rigidbody2D>();
        targetReticle.SetActive(false);
        targetResetPos = targetReticle.transform.localPosition;
        initialRingPosition = ring.transform.position;
        initialRingRotation = ring.transform.rotation;
    }

    void Update()
    {
        HandleThrowing();
    }

    private void HandleThrowing()
    {
        if (Input.GetButtonDown("Fire1") && !aiming)
        {
            targetReticle.SetActive(true);
            aiming = true;
        }
        else if (Input.GetButtonDown("Fire1") && aiming)
        {
            Vector2 throwDirection = targetReticle.transform.position - transform.position;
            targetReticle.SetActive(false);
            ringRB.isKinematic = false;
            ringRB.velocity = throwDirection.normalized * throwForceForward;
            ringRB.angularVelocity += ringSpin;
            aiming = false;
            targetReticle.transform.localPosition = targetResetPos;
        }

        if (!aiming) return;

        // Handle movement of the target reticle (if needed)
        targetReticle.transform.RotateAround(transform.position, Vector3.forward, targetMoveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == ring)
        {
            // Ring is picked up, reset its state
            ResetRing();
        }
    }

    private void ResetRing()
    {
        // Reset position and rotation of the ring
        ring.transform.position = initialRingPosition;
        ring.transform.rotation = initialRingRotation;

        // Disable physics and reset velocity
        ringRB.velocity = Vector2.zero;
        ringRB.angularVelocity = 0f;
        ringRB.isKinematic = true;

        // Reset aiming state and hide reticle
        aiming = false;
        targetReticle.SetActive(false);
    }
}
