using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 8f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private bool flutterUsed = false;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            Debug.Log("Spacebar pressed");
            flutterUsed = false;
        }

        else if (Input.GetButtonDown("Jump") && !IsGrounded() && !flutterUsed)
        {
            rb.velocity = new Vector2 (rb.velocity.x, jumpingPower * 0.25f);
            Debug.Log("Flutter jump triggered");
            flutterUsed = true;
        }

        /*
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            Debug.Log("Spacebar up");
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.25f);
            Debug.Log("Spacebar up, velocity: " + rb.velocity);
        }
        */


        Flip();
    }  

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
