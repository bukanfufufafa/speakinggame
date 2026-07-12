using UnityEngine;

public class AttakHitBox : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            enemyStatus enemy = collision.GetComponent<enemyStatus>();

            if (enemy != null)
            {
                enemy.takeDamage(damage);
            }
        }
    }
}