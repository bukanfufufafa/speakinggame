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
    private SkillManager skillManager;
    private characterStat stats;

    // Tambahkan properti helper agar kode lebih bersih
    private bool IsBusy => (playerAttack != null && playerAttack.isAttacking) || (skillManager != null && skillManager.SedangCasting);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerAttack = GetComponent<PlayerAttack>();
        skillManager = GetComponent<SkillManager>();
        stats = GetComponent<characterStat>();
    }

    public int GetDirection()
    {
        return direction;
    }

    void Update()
    {
        if (IsBusy)
        {
            moveInput = 0;
            fallThrough = false;
            // Jika sedang casting/attack, kita paksa velocity X langsung 0 di sini juga agar responsif
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            // Input kiri kanan menggunakan Input.GetAxisRaw agar stop-and-go lebih tajam
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

        // Set parameter animasi lari
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
        // Jika sedang sibuk casting/menyerang, jangan biarkan ada kalkulasi pergerakan baru
        if (IsBusy)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

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