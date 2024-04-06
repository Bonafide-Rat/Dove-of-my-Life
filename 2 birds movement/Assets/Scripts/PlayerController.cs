using TarodevController;
using Unity.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Movement variables
    [SerializeField] private ScriptableStats stats;
    private float speed;
    private float jumpingPower;
    private float acceleration;
    private float currentSpeed;
    private float groundDeceleration;
    private bool isFacingRight = true;
    private Vector2 movementInput;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    #endregion

    #region Jump variables
    private int jumpCount = 0;
    private bool birdGrounded = true;
    private float lastGroundedTime;
    private float coyoteTime;
    private float jumpBuffer;
    private float lastJumpTime;
    #endregion

    // Assign values from stats script
    private void Awake()
    {
        // Basic movement variables:
        speed = stats.MaxSpeed;
        jumpingPower = stats.JumpPower;
        acceleration = stats.Acceleration;
        groundDeceleration = stats.GroundDeceleration;

        // Jumping variables:
        coyoteTime = stats.CoyoteTime;
        jumpBuffer = stats.JumpBuffer;  
    }

    void Update()
    {
        getInput();
        handleJump();
        Flip();
    }

    private void FixedUpdate()
    {
        handleHorizontalMovement();
        HandleGravity();
    }

    #region Player Movement:
    private void getInput()
    {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);
        // Record the time when jump button is pressed
        if (Input.GetButtonDown("Jump"))
        {
            lastJumpTime = Time.time;
        }
    }

    private void handleHorizontalMovement()
    {
        // Handle horizontal movement with acceleration and deceleration.
        float targetSpeed = movementInput.x * speed;  // Desired speed based on input.
        // Interpolate currentSpeed towards targetSpeed based on acceleration or deceleration rates.
        if (Mathf.Abs(movementInput.x) > 0.01f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, groundDeceleration * Time.fixedDeltaTime);
        }

        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    #endregion

    #region Jumping Logic
    
    private bool jumpHeld = false;
    private void handleJump()
    {
        birdGrounded = IsGrounded();

        if (birdGrounded)
        {
            lastGroundedTime = Time.time;
            rb.gravityScale = 1;
            jumpCount = 0; // Reset jumpCount when grounded
            jumpHeld = false;

            // Check for buffered jump input
            if (Time.time - lastJumpTime <= jumpBuffer)
            {
                PerformJump(1);
            }
        }
        else if (Time.time - lastGroundedTime > coyoteTime)
        {
            jumpCount = Mathf.Max(1, jumpCount);
        }

        // Normal jump input check moved here to avoid duplicating PerformJump calls
        if (!birdGrounded && Input.GetButtonDown("Jump") && jumpCount < 2 && (Time.time - lastGroundedTime <= coyoteTime))
        {
            PerformJump(jumpCount + 1);
        }
    }

    private void PerformJump(int jumpType)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

        switch (jumpType)
        {
            case 1: // Standard Jump
                rb.gravityScale = 2;
                break;
            case 2: // Flutter Jump
                rb.gravityScale = 4;
                break;
        }
        jumpCount = jumpType;
    }

    #endregion

    #region Gravity
    private void HandleGravity()
    {
        if (birdGrounded && rb.velocity.y <= 0f)
        {
            // When grounded and not moving upwards, apply a grounding force to keep the player on the ground.
            rb.velocity = new Vector2(rb.velocity.x, stats.GroundingForce);
        }
        else
        {
            // Calculate in-air gravity.
            var inAirGravity = stats.FallAcceleration;

            // Check if the jump has ended early (the player has released the jump button before reaching the apex of the jump).
            if ((!jumpHeld || jumpCount > 0) && rb.velocity.y > 0)
            {
                inAirGravity *= stats.JumpEndEarlyGravityModifier;
            }

            // Apply gravity towards the max fall speed.
            rb.velocity = new Vector2(rb.velocity.x, Mathf.MoveTowards(rb.velocity.y, -stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime));
        }
    }
    #endregion

    #region Collisions
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    #endregion

    private void Flip()
    {
        if (isFacingRight && rb.velocity.x < 0f || !isFacingRight && rb.velocity.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
    }
#endif

}
