using UnityEngine;

public class WaterBallProjectile : MonoBehaviour
{
    private Vector2 arahHadap;
    private float speed;
    private float damage;
    
    public void Inisialisasi(Vector2 arahTembak, float speedPeluru, float dmg)
    {
        arahHadap = arahTembak.normalized;
        speed = speedPeluru;
        damage = dmg;
    }

    void Update()
    {
        transform.Translate( arahHadap * speed * Time.deltaTime, Space.World);
        Destroy(gameObject,1.5f);

    }

    void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("enemy"))
    {
        enemyStatus enemy = collision.GetComponent<enemyStatus>();
        if (enemy != null)
        {
            enemy.takeDamage((int)damage);
        }
        Destroy(gameObject);
    }
}
}
