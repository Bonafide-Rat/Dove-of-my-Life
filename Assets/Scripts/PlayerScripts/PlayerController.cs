using System;
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
    public static bool isFacingRight = true;
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
    private float lastJumpTime = -1000f;
    private bool jumpHeld = false;
    private float airDeceleration;
    private float jumpEndEarlyGravity;
    private float fallAcceleration;
    private bool isGliding = false;
    // private bool shiftWasPressed = false;
    #endregion

    private bool insideAreaEffector = false;

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
        // HandleGravity();

        if (!insideAreaEffector)
        {
            HandleGravity();
        }

    }

    #region Player Movement
    private void getInput()
    {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);

        // Record the time when jump button is pressed for the jump buffer
        if (Input.GetButtonDown("Jump"))
        {
            lastJumpTime = Time.time;
            isGliding = false;
        }

        bool shiftPressed = Input.GetKeyDown(KeyCode.LeftShift);

        // Toggle gliding on Shift press (only if not grounded to prevent toggling while walking)
        if (Input.GetKeyDown(KeyCode.LeftShift) && !birdGrounded)
        {
            isGliding = !isGliding; // Toggle gliding state
        }

        // Optionally, reset gliding when grounded or under other conditions
        if (birdGrounded)
        {
            isGliding = false;
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
            //This code: (Time.time - lastJumpTime <= jumpBuffer) causes player to Jump when game is started. 
            if (jumpButtonPressed || (Time.time - lastJumpTime <= jumpBuffer))
            {
                PerformJump(1);
                jumpHeld = true;
                isGliding = false;
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
                //Debug.Log("Regular Jump Pressed!");
                break;
            case 2: // Double jump
                rb.gravityScale = 1;
                //Debug.Log("Double jump pressed!");
                break;
        }
        jumpCount = jumpType;
        isGliding = false;
    }

    #endregion

    #region Gravity
    private void HandleGravity()
    {
        // Case 1 - Gravity when grounded:
        if (birdGrounded && rb.velocity.y <= 0f)
        {
            // When grounded and not moving upwards, apply a grounding force to keep the player on the ground.
            rb.velocity = new Vector2(rb.velocity.x, stats.GroundingForce);

            // Reset gravity scale once grounded
            rb.gravityScale = 1;
        }
        // Case 2 - Gravity when gliding:
        else if (isGliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -stats.GlideFallSpeed);
        }
        // Case 3 - Gravity when jump is released early (short jumps):
        else
        {
            float inAirGravity = fallAcceleration;

            // Check if the jump has ended early (the player has released the jump button before reaching the apex of the jump).
            if ((!jumpHeld && jumpCount > 0) && rb.velocity.y > 0)
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WindZone"))
        {
            insideAreaEffector = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("WindZone"))
        {
            insideAreaEffector = false;
        }
    }

    #endregion

    private void Flip()
    {
        if (isFacingRight && Input.GetAxisRaw("Horizontal") < 0f || !isFacingRight && Input.GetAxisRaw("Horizontal") > 0f)
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