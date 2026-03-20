using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    private bool movingRight = true;

    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;

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

    void Flip()
    {
        movingRight = !movingRight;
        transform.eulerAngles = new Vector3(0, movingRight ? 0 : 180, 0);
    }
}