using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private characterStat stats;

    private int currentHealth;
    public int CurrentHealth => currentHealth;
    // Start is called before the first frame update
    private void Start()
    {
      currentHealth = stats.MaxHealth;   
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
            Die();
    }
    //bisa nanti tambahin heal disini
    public void Die()
    {
        Debug.Log("Player Mati");
    }
}
