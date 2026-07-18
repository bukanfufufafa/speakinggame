using UnityEngine;

public class enemyDetector : MonoBehaviour
{
    [SerializeField] EnemyRoutine enemyRoutine;
    [SerializeField] enemyStatus status;
    [SerializeField] private bool chasingPlayer = false;

    private PlayerSneak trackedSneak;

    void Start()
    {
        enemyRoutine = GetComponent<EnemyRoutine>();
        status = GetComponent<enemyStatus>();
    }

    void Update()
    {
        bool blockedBySneak = status != null
            && status.EnemyType == EnemyType.Human
            && trackedSneak != null
            && trackedSneak.IsHiding;

        if (chasingPlayer && !blockedBySneak)
        {
            enemyRoutine.Chasing();
        }
        else
        {
            enemyRoutine.ReturnToStart();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chasingPlayer = true;
            trackedSneak = collision.GetComponent<PlayerSneak>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chasingPlayer = false;
            trackedSneak = null;
        }
    }
}