using System.Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyRoutine : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float targetX;
    [SerializeField] private float delayTime = 5f;

    [Header("Chasing")]
    [SerializeField] private float rangeChasing = 5f;

    public Animator anim;

    private Rigidbody2D rb;
    private float startX;

    private bool isAttacking;

    // Batas patroli
    private float patrolMinX;
    private float patrolMaxX;

    private bool movingToTarget = true;
    private bool isWaiting;
    private bool isReturning;

    private float idleTimer;
    private int direction = 1;

    private bool attackPlaying;

    [SerializeField] private enemyStatus enemyStatus;
    public bool detectPlayer = false;

    public int GetDirection()
    {
        return direction;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (enemyStatus == null)
            enemyStatus = GetComponent<enemyStatus>();

        startX = rb.position.x;

        patrolMinX = Mathf.Min(startX, targetX);
        patrolMaxX = Mathf.Max(startX, targetX);

        direction = (targetX > startX) ? 1 : -1;
    }

    private void FixedUpdate()
    {
        if (isReturning)
        {
            ReturnToStart();
        }
        else if (detectPlayer)
        {
            //Chasing();
            attackingIdle();
          
        }
        else
        {
            if (isWaiting)
                Idle();
            else
                Patrol();
        }

        Flip();
    }

    private void Patrol()
    {
        anim.SetBool("isIdle", false);

        float destinationX = movingToTarget ? targetX : startX;

        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            new Vector2(destinationX, rb.position.y),
            moveSpeed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition);

        direction = destinationX > rb.position.x ? 1 : -1;

        if (Mathf.Abs(rb.position.x - destinationX) < 0.05f)
        {
            isWaiting = true;
            idleTimer = delayTime;
            anim.SetBool("isIdle", true);
        }
    }

    private void Idle()
    {
        idleTimer -= Time.fixedDeltaTime;

        if (idleTimer <= 0f)
        {
            isWaiting = false;
            movingToTarget = !movingToTarget;

            direction = movingToTarget
                ? (targetX > startX ? 1 : -1)
                : (targetX > startX ? -1 : 1);

            anim.SetBool("isIdle", false);
        }
    }

    private void attackingIdle()
    {
        // Diam di tempat
        rb.MovePosition(rb.position);

        // Tetap menghadap player
        if (enemyStatus.playerPos != null)
        {
            direction = enemyStatus.playerPos.position.x > rb.position.x ? 1 : -1;
        }


        anim.SetBool("isIdle", false);
    }

    private void Chasing()
    {
        if (enemyStatus.playerPos == null)
        {
            detectPlayer = false;
            isReturning = true;
            return;
        }

        float minX = patrolMinX - rangeChasing;
        float maxX = patrolMaxX + rangeChasing;

        if (rb.position.x < minX || rb.position.x > maxX)
        {
            detectPlayer = false;
            isReturning = true;
            return;
        }

        Vector2 targetPos = enemyStatus.playerPos.position;

        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            targetPos,
            moveSpeed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition);

        direction = targetPos.x > rb.position.x ? 1 : -1;

        anim.SetBool("isIdle", false);
    }

    private void ReturnToStart()
    {
        Vector2 target = new Vector2(startX, rb.position.y);

        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            target,
            moveSpeed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition);

        direction = startX > rb.position.x ? 1 : -1;

        if (Mathf.Abs(rb.position.x - startX) < 0.05f)
        {
            isReturning = false;
            movingToTarget = true;
            isWaiting = false;
            detectPlayer = false;
        }
    }

    private void Flip()
    {
        if (direction == 1)
            transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        else
            transform.localScale = new Vector3(-0.4f, 0.4f, 0.4f);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.yellow;

        float minX = patrolMinX - rangeChasing;
        float maxX = patrolMaxX + rangeChasing;

        Gizmos.DrawLine(
            new Vector3(minX, transform.position.y),
            new Vector3(maxX, transform.position.y));

        Gizmos.DrawSphere(
            new Vector3(minX, transform.position.y),
            0.1f);

        Gizmos.DrawSphere(
            new Vector3(maxX, transform.position.y),
            0.1f);
    }

    public void attacking()
    {
        attackingIdle();

        if (!attackPlaying)
        {
            attackPlaying = true;
            anim.SetBool("isAttack", true);
        }
    }

    public void EndAttack()
    {
        attackPlaying = false;
        anim.SetBool("isAttack", false);
    }
}