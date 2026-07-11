using UnityEngine;

public class AttakHitBox : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            collision.GetComponent<enemyStatus>()?.takeDamage(damage);
        }     
    }
}
