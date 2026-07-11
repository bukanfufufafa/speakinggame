using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyGravity : MonoBehaviour
{
    [SerializeField] private float gravity = 30f;
    [SerializeField] private float maxFallSpeed = 20f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.1f;

    private Rigidbody2D rb;
    private float verticalVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        bool grounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer);

        if (grounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = 0;
        }
        else
        {
            verticalVelocity -= gravity * Time.fixedDeltaTime;
            verticalVelocity = Mathf.Max(verticalVelocity, -maxFallSpeed);
        }

        rb.MovePosition(rb.position + Vector2.up * verticalVelocity * Time.fixedDeltaTime);
    }
}