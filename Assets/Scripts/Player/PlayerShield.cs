using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [Header("Setting")]
    public float cooldown = 2f;
    public Color shieldTintColor = new Color(0.7f, 0.85f, 1f);

    private entity_status status;
    private SkillManager skillManager;
    private bool sedangShield = false;
    private float cooldownTersisa = 0f;

    private void Awake()
    {
        status = GetComponent<entity_status>();
        skillManager = GetComponent<SkillManager>();
        status.OnShieldHit += HandleShieldHit;
    }

    private void OnDestroy() => status.OnShieldHit -= HandleShieldHit;

    private void Update()
    {
        if (cooldownTersisa > 0f) cooldownTersisa -= Time.deltaTime;

        bool tombolDitekan = Input.GetMouseButton(1); // RMB

        if (!sedangShield && tombolDitekan)
            CobaAktifkanShield();
        else if (sedangShield && !tombolDitekan)
            NonaktifkanShield();
    }

    private void CobaAktifkanShield()
    {
        if (skillManager.SedangCasting) return;
        if (cooldownTersisa > 0f) return;

        sedangShield = true;
        status.SetShielding(true);
        status.SetRegenTint(true, shieldTintColor);
    }

    private void NonaktifkanShield()
    {
        if (!sedangShield) return;

        sedangShield = false;
        status.SetShielding(false);
        status.SetRegenTint(false, Color.white);
        cooldownTersisa = cooldown;
    }

    private void HandleShieldHit(bool isHeavyAttack)
    {
        if (!sedangShield) return;

        sedangShield = false;
        status.SetShielding(false);
        status.SetRegenTint(false, Color.white);
        cooldownTersisa = cooldown;

        Debug.Log(isHeavyAttack
            ? "Shield tembus, kena heavy attack"
            : "Shield pecah, serangan berhasil diblok");
    }
}