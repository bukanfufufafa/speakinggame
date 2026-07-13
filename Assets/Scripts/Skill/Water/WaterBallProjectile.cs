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
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            //TODO enemy terima damage other.GetComponent<Kesehatan>().TerimaDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject,5f);
        }
    }
}
