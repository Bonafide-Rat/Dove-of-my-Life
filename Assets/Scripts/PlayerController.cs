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
    private bool jumpHeld = false;
    private float airDeceleration;
    private float jumpEndEarlyGravity;
    private float fallAcceleration;
    #endregion

    // Assign values from stats script
    private void Awake()
    {
        // Basic movement variables:
        speed = stats.MaxSpeed;
        jumpingPower = stats.JumpPower;
        acceleration = stats.Acceleration;
        groundDeceleration = stats.GroundDeceleration;

        // Jumping & airtime variables:
        coyoteTime = stats.CoyoteTime;
        jumpBuffer = stats.JumpBuffer;
        airDeceleration = stats.AirDeceleration;
        fallAcceleration = stats.FallAcceleration;
        jumpEndEarlyGravity = stats.JumpEndEarlyGravityModifier;
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

    #region Player Movement
    private void getInput()
    {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);
        
        // Record the time when jump button is pressed for the jump buffer
        if (Input.GetButtonDown("Jump"))
        {
            lastJumpTime = Time.time;
        }
    }

    private void handleHorizontalMovement()
    {
        // Check if there is horizontal movement input.
        if (Mathf.Abs(movementInput.x) > 0.01f)
        {
            // If there is input, move towards the target speed using the specified acceleration.
            currentSpeed = Mathf.MoveTowards(currentSpeed, movementInput.x * speed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // If there is no input, decelerate towards 0 using either ground or air deceleration.
            float deceleration = IsGrounded() ? groundDeceleration : airDeceleration;
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
        }

        // Apply the calculated horizontal speed and maintain the current vertical speed.
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    #endregion

    #region Jumping Logic

    private void handleJump()
    {
        birdGrounded = IsGrounded();
        bool jumpButtonPressed = Input.GetButtonDown("Jump");

        if (birdGrounded)
        {
            lastGroundedTime = Time.time;
            if (jumpButtonPressed || (Time.time - lastJumpTime <= jumpBuffer))
            {
                PerformJump(1);
                jumpHeld = true;
            }
        }
        else if (!birdGrounded && jumpButtonPressed && jumpCount < 2 && (Time.time - lastGroundedTime <= coyoteTime || Time.time - lastJumpTime <= jumpBuffer))
        {
            PerformJump(jumpCount + 1);
            jumpHeld = true;
        }

        if (jumpButtonPressed) // For jump buffer
        {
            lastJumpTime = Time.time;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.gravityScale = jumpEndEarlyGravity;

            jumpHeld = false;
        }
    }

    private void PerformJump(int jumpType)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

        switch (jumpType)
        {
            case 1: // Standard Jump
                rb.gravityScale = 1;
                Debug.Log("Regular Jump Pressed!");
                break;
            case 2: // Flutter Jump
                rb.gravityScale = 3;
                Debug.Log("Flutter jump pressed!");
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
            float inAirGravity = fallAcceleration;

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
