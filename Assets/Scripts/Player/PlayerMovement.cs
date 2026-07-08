using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public Animator anim;
    [Header("Ground")]
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    [SerializeField] private bool isGrounded;
    private float moveInput;
    public SpriteRenderer sprite;
    private int direction = 1;
    private PlayerAttack playerAttack;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerAttack = GetComponent<PlayerAttack>();
    }
    public int GetDirection()
    {
        return direction;
    }

    void Update()
    {
        if (playerAttack != null && playerAttack.isAttacking)
        {
            moveInput = 0; // paksa diam selagi nyerang
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
        }

        anim.SetBool("running", moveInput != 0);
        //flip karakter
        if (direction == 1)
        {
            transform.localScale = new Vector3(0.4f, 0.4f, 1);
        }
        else if (direction == -1)
        {
            transform.localScale = new Vector3(-0.4f, 0.4f, 1);
        }
        //animasi
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);

        // Lompat
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Gerakan horizontal
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
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
