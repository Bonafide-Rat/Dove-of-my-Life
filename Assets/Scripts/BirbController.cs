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

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            jumpCount = 0;
            rb.gravityScale = 1;
        }

        if (Input.GetButtonDown("Jump") && jumpCount == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            Debug.Log("Spacebar pressed");
            jumpCount++;
        }

        if (Input.GetButtonDown("Jump") && jumpCount == 1)
        {
            rb.gravityScale = 2;
            rb.velocity = new Vector2 (rb.velocity.x, jumpingPower);
            Debug.Log("Flutter jump triggered");
            jumpCount++;
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