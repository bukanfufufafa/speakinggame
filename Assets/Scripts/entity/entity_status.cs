using System.Collections;
using UnityEngine;

public class entity_status : MonoBehaviour
{
    public float maxHealth = 300;
    public float maxMana = 50;
    public float health;
    public float mana;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    private Coroutine damageFlashCoroutine;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        health = maxHealth;
        mana = maxMana;
    }

    public void takeDamage(float damage)
    {
        health -= damage;

        if (damageFlashCoroutine != null)
        {
            StopCoroutine(damageFlashCoroutine);
        }

        damageFlashCoroutine = StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        spriteRenderer.color = Color.white;

        damageFlashCoroutine = null;
    }
}