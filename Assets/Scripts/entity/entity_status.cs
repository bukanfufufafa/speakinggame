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
    private Coroutine flashCoroutine;
    private bool sedangRegenTint = false;
    private Color currentRegenColor = Color.white;

    public event Action OnDamaged;
    public event Action OnDied;
    public event Action<bool> OnShieldHit; // true = heavy attack, false = normal attack

    public bool IsRooted { get; private set; }
    public bool IsShielding { get; private set; }

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
        mana = maxMana;
    }

    public void takeDamage(float damage, bool isHeavyAttack = false)
    {
        if (damage <= 0f || health <= 0f) return;

        if (IsShielding)
        {
            OnShieldHit?.Invoke(isHeavyAttack);

            if (!isHeavyAttack)
            {
                return;
            }
        }

        health = Mathf.Max(0f, health - damage);
        FlashColor(Color.red);

        OnDamaged?.Invoke();

        if (health <= 0f)
            OnDied?.Invoke();
    }

    private void FlashColor(Color color)
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRoutine(color));
    }

    private IEnumerator FlashRoutine(Color color)
    {
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.2f);

        flashCoroutine = null;
        spriteRenderer.color = sedangRegenTint ? currentRegenColor : Color.white;
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

    public void SetShielding(bool active)
    {
        IsShielding = active;
    }

    public void SetRegenTint(bool active, Color color)
    {
        sedangRegenTint = active;
        currentRegenColor = active ? color : Color.white;

        if (spriteRenderer == null || flashCoroutine != null) return;
        spriteRenderer.color = currentRegenColor;
    }
}