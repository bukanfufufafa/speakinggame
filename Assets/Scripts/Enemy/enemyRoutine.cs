using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyRoutine : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float targetX;

    private Rigidbody2D rb;
    private float startX;
    private bool movingToTarget = true;

    private int direction = 1;

    public int GetDirection()
    {
        return direction;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        startX = rb.position.x;

       
        direction = (targetX > startX) ? 1 : -1;
    }

    private void FixedUpdate()
    {
        float destinationX = movingToTarget ? targetX : startX;

        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            new Vector2(destinationX, rb.position.y),
            moveSpeed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition);

        if (Mathf.Abs(rb.position.x - destinationX) < 0.05f)
        {
            movingToTarget = !movingToTarget;

            
            direction = movingToTarget ? (targetX > startX ? 1 : -1)
                                       : (targetX > startX ? -1 : 1);
        }

        if (direction == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}