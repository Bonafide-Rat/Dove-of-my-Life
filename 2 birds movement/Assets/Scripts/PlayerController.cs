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

    private int jumpCount = 0;
    private bool birdGrounded = true;
    private float lastGroundedTime;
    private float coyoteTime = 0.5f;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        Debug.Log(jumpCount);

        if (IsGrounded())
        {
            lastGroundedTime = Time.time;
            rb.gravityScale = 1;
            birdGrounded = true;
        }
        else
        {
            birdGrounded = false;
            if (Time.time - lastGroundedTime > coyoteTime)
            {
                // Ensures jumpCount is at least 1 if coyote time has passed, preventing direct flutter jumps
                jumpCount = Mathf.Max(1, jumpCount);
            }
        }

        bool canStandardJump = jumpCount == 0 && (birdGrounded || Time.time - lastGroundedTime <= coyoteTime);
        bool canFlutterJump = jumpCount == 1;

        if (Input.GetButtonDown("Jump") && canStandardJump)
        {
            rb.gravityScale = 2;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            Debug.Log("Regular jump triggered");
            jumpCount++;
        }
        else if (Input.GetButtonDown("Jump") && canFlutterJump)
        {
            rb.gravityScale = 4;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            Debug.Log("Flutter jump triggered");
            jumpCount++;
        }

        if (birdGrounded)
        {
            jumpCount = 0; // Reset jumpCount when grounded
        }

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
