using UnityEngine;

public class enemyStatus : entity_status
{
    public float damage;
    public Collider2D detector;
    [SerializeField] private EnemyRoutine enemyRoutine;
    public Transform playerPos;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyRoutine.detectPlayer = true;
            playerPos = other.GetComponent<Transform>();
            enemyRoutine.attacking();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        enemyRoutine.detectPlayer = false;
        playerPos = null;
        enemyRoutine.EndAttack();
    }
}