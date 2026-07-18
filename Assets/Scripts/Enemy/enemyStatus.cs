using UnityEngine;

public enum EnemyType
{
    Human,
    NonHuman
}

public class enemyStatus : entity_status
{
    [Header("Attack")]
    public float damage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    private float attackTimer;

    [Header("Type")]
    [SerializeField] private EnemyType enemyType = EnemyType.Human;
    public EnemyType EnemyType => enemyType;

    [Header("References")]
    [SerializeField] private EnemyRoutine enemyRoutine;
    [SerializeField] private PlayerHealth playerhealth;

    public Transform playerPos;

    private PlayerSneak trackedSneak;

    private void Start()
    {
        attackTimer = attackCooldown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSneak sneak = other.GetComponent<PlayerSneak>();

            // Enemy Human tidak bisa mendeteksi player yang sedang hide
            if (enemyType == EnemyType.Human && sneak != null && sneak.IsHiding)
                return;

            enemyRoutine.detectPlayer = true;
            playerPos = other.transform;
            playerhealth = other.GetComponent<PlayerHealth>();
            trackedSneak = sneak;

            enemyRoutine.attacking();
        }
    }

    private void Update()
    {
        Attack();
        CheckSneakBreak();
        if (health <= 0)
        {
            die();
        }
    }

    // Kalau player mulai hide di tengah-tengah dikejar/diserang, enemy Human
    // langsung kehilangan target
    private void CheckSneakBreak()
    {
        if (enemyType != EnemyType.Human) return;
        if (trackedSneak == null) return;

        if (trackedSneak.IsHiding)
        {
            LosePlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LosePlayer();
        }
    }

    private void LosePlayer()
    {
        enemyRoutine.detectPlayer = false;
        playerPos = null;
        playerhealth = null;
        trackedSneak = null;

        enemyRoutine.EndAttack();

        attackTimer = attackCooldown;
    }

    private void Attack()
    {
        if (playerhealth == null)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            playerhealth.takeDamage(damage);
            attackTimer = attackCooldown;
        }
    }

    private void die()
    {
        gameObject.SetActive(false);
    }
}