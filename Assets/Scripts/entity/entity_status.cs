using System;
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

    public event Action OnDamaged;
    public event Action OnDied;

    public bool IsRooted { get; private set; }

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        health = maxHealth;
        mana = maxMana;
    }

    public void takeDamage(float damage)
    {
        if (damage <= 0f || health <= 0f) return;
        health = Mathf.Max(0f, health - damage);
        if (damageFlashCoroutine != null)
            StopCoroutine(damageFlashCoroutine);
        damageFlashCoroutine = StartCoroutine(DamageFlash());
        OnDamaged?.Invoke();
        if (health <= 0f)
            OnDied?.Invoke();
    }

    private IEnumerator DamageFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
        damageFlashCoroutine = null;
    }

    public void RestoreHealth(float amount)
    {
        health = Mathf.Min(maxHealth, health + amount);
    }

    public bool SpendMana(float amount)
    {
        if (mana < amount) return false;
        mana -= amount;
        return true;
    }

    public void RestoreMana(float amount)
    {
        mana = Mathf.Min(maxMana, mana + amount);
    }

    public void SetRooted(bool rooted)
    {
        IsRooted = rooted;
    }
}