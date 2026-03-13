using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody2D playerRb;
    public float speed = 8f;
    public float acceleration = 10f;
    public float deceleration = 15f;

    [Header("Jump")]
    public float jumpForce = 14f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform feetPosition;
    public float groundCheckCircle = 0.2f;

    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    private float horizontal;
    private bool isGrounded;
    private float coyoteCounter;
    private float jumpBufferCounter;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Sprite flip
        if (horizontal < 0) spriteRenderer.flipX = true;
        else if (horizontal > 0) spriteRenderer.flipX = false;

        // Ground check
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, groundLayer);

        // Animator parameters
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("YVelocity", playerRb.linearVelocity.y);

        // Coyote time
        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        // Jump buffer
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // Jump uitvoeren
        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
        }

        // Variabele jump hoogte
        if (playerRb.linearVelocity.y < 0)
        {
            playerRb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerRb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            playerRb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = horizontal * speed;
        float speedDif = targetSpeed - playerRb.linearVelocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = speedDif * accelRate;

        playerRb.AddForce(Vector2.right * movement);
    }

    void OnDrawGizmosSelected()
    {
        if (feetPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(feetPosition.position, groundCheckCircle);
        }
    }
}


