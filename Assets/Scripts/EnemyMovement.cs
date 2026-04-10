using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    private bool movingRight = true;

    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;

    [Header("Stomp Settings")]
    [Tooltip("Hoe hard de speler omhoog bounced na het stompen")]
    public float stompBounceForce = 10f;

    void Update()
    {
        // Bewegen
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Check of er nog grond is
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        if (!groundInfo.collider)
        {
            Flip();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Controleer of de speler van bovenaf op de vijand landt
        // door te kijken of het contactpunt boven het midden van de vijand zit
        // en de speler naar beneden beweegt
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                // Speler springt op de vijand — geef een bounce omhoog
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, stompBounceForce);
                }

                // Vijand gaat dood
                Destroy(gameObject);
                return;
            }
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.eulerAngles = new Vector3(0, movingRight ? 0 : 180, 0);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }
    }
}
