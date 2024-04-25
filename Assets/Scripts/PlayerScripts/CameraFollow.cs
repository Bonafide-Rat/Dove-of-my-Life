using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private Transform target;


    public float moveSpeed = 10f; // Speed of camera movement

    [SerializeField] private bool CameraMoving = false;
    [SerializeField] private float moveInput;
    


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");

        if (moveInput == 0){
            CameraUpdate();
        }

        if (!IsMoving() && IsGrounded())
        {
            ManualLook();
        }
    }

    private void CameraUpdate() 
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    private bool IsGrounded() 
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsMoving() 
    {
        return target.GetComponent<Rigidbody2D>().velocity.magnitude != 0;
    }

    private void ManualLook()
    {
        // Calculate the new position of the camera
        Vector3 newPosition = transform.position + Vector3.up * moveInput * moveSpeed * Time.deltaTime;
        // Update the position of the camera
        transform.position = newPosition;
    }

}
