using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    [Range(0f, 1f)] public float jumpCut = 0.5f;
    public Animator anim;

    [Header("Ground")]
    public LayerMask groundLayer;
    public bool fallThrough = false;

    private Rigidbody2D rb;
    [SerializeField] private bool isGrounded;
    private float moveInput;
    public SpriteRenderer sprite;
    private int direction = 1;
    private PlayerAttack playerAttack;
    private characterStat stats;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerAttack = GetComponent<PlayerAttack>();
        stats = GetComponent<characterStat>();
    }

    public int GetDirection()
    {
        return direction;
    }

    void Update()
    {
        if (playerAttack != null && playerAttack.isAttacking)
        {
            moveInput = 0;
            fallThrough = false;
        }
        else
        {
            // Input kiri kanan
            if (Input.GetKey(KeyCode.A))
            {
                moveInput = -1;
                direction = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveInput = 1;
                direction = 1;
            }
            else
            {
                moveInput = 0;
            }

            // Lompat
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            // Jump Cut
            if (Input.GetButtonUp("Jump"))
            {
                if (rb.velocity.y > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCut);
                }
            }

            // Flag untuk drop melalui platform
            fallThrough = Input.GetKey(KeyCode.S);
        }

        anim.SetBool("running", moveInput != 0);

        // Flip karakter
        if (direction == 1)
        {
            transform.localScale = new Vector3(0.4f, 0.4f, 1);
        }
        else if (direction == -1)
        {
            transform.localScale = new Vector3(-0.4f, 0.4f, 1);
        }

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    void FixedUpdate()
    {
        float currentSpeed = stats != null ? stats.MoveSpeed : moveSpeed;
        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
}