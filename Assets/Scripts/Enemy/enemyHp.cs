using UnityEngine;

public class enemyHp : MonoBehaviour
{
    public int maxHp = 30;
    public int currentHp;
    void Start()
    {
        currentHp = maxHp;
    }

    public void takeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log("Enemy terkena hit" + damage);
        
        if(currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
