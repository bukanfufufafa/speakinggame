using UnityEngine;

public class enemyStatus : entity_status
{
    [Header("Attack")]
    public float damage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    private float attackTimer;

    [Header("References")]
    [SerializeField] private EnemyRoutine enemyRoutine;
    [SerializeField] private PlayerHealth playerhealth;

    public Transform playerPos;

    private void Start()
    {
        attackTimer = attackCooldown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyRoutine.detectPlayer = true;
            playerPos = other.transform;
            playerhealth = other.GetComponent<PlayerHealth>();

            enemyRoutine.attacking();
        }
    }

    

    private void Update()
    {
        Attack();
        if (health <= 0)
        {
            die();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyRoutine.detectPlayer = false;
            playerPos = null;
            playerhealth = null;

            enemyRoutine.EndAttack();


            attackTimer = attackCooldown;
        }
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