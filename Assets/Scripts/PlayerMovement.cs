using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float acceleration = 2f;
    public float deceleration = 2f;
    public float jumpForce = 5f;
    public float coyoteTime = 0.2f;
    public float ledgeHopHeight = 1f;
    public float jumpBufferTime = 0.2f;  // Time to buffer jump input
    public float debugMaxSpeed = 8f;     // Max speed for debug mode
    public float debugAcceleration = 20f;// Acceleration for debug mode
    public float debugDeceleration = 20f;// Deceleration for debug mode

    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float currentSpeed = 0f;
    private bool isJumping = false;
    private float coyoteTimeCounter = 0f;
    private bool isGrounded = false;
    private float jumpBufferCounter = 0f;
    private bool debugMode = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        // Toggle debug mode
        if (Input.GetKeyDown(KeyCode.F9))
        {
            debugMode = !debugMode;
        }

        // Get horizontal input
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate target speed based on input
        float targetSpeed = horizontalInput * (debugMode ? debugMaxSpeed : maxSpeed);

        // Smoothly transition to the target speed
        if (horizontalInput != 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (debugMode ? debugAcceleration : acceleration) * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, (debugMode ? debugDeceleration : deceleration) * Time.deltaTime);
        }

        // Apply horizontal velocity to the Rigidbody2D
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

        // Check for jump input and coyote time
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if ((isGrounded || coyoteTimeCounter > 0) && jumpBufferCounter > 0)
        {
            isJumping = true;
            jumpBufferCounter = 0;
        }

        // Update coyote time counter
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Decrease jump buffer counter
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isJumping)
        {
            // Apply jump force
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = false;
        }

        // Ledge hop check
        LedgeHopCheck();
    }

    void LedgeHopCheck()
    {
        // Check for ledge hop conditions here
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(currentSpeed), 0.5f, groundLayer);
        if (hit.collider != null && Mathf.Abs(hit.normal.y) < 0.1f)
        {
            Vector3 ledgeHopPosition = hit.point;
            ledgeHopPosition.y += ledgeHopHeight;
            transform.position = ledgeHopPosition;
        }
    }
}
