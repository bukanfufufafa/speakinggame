using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : entity_status
{
    [SerializeField] private characterStat stats;
    public Slider healthBar;
    public Slider manaBar;

    protected override void Start()
    {
        base.Start();

        healthBar.maxValue = maxHealth;
        healthBar.value = health;

        manaBar.maxValue = maxMana;
        manaBar.value = mana;
    }

    public void Die()
    {
        Debug.Log("Player Mati");
    }

    private void Update()
    {
        healthBar.value = health;
        manaBar.value = mana;

        if (health <= 0)
        {
            SceneManager.LoadScene("gameover");
        }
    }
}