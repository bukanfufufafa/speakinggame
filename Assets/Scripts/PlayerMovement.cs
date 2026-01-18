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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();   
    }

    void Update()
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
        
        
        if (moveInput != 0)
        {
            anim.SetBool("running", true);
        }
        else
        {
            anim.SetBool("running", false);
        }

        if (direction == 1)
        {
            sprite.flipX = false;
        }
        else if (direction == -1)
        {
            sprite.flipX = true;
        }
           
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
