using UnityEngine;

public class PlayerHPRegen : MonoBehaviour
{
    [Header("Setting")]
    public float castTime = 2f;
    public float manaCost = 30f;
    public float healAmount = 40f;
    public float cooldown = 10f;
    public KeyCode key = KeyCode.Alpha1;
    public Color regenTintColor = new Color(0.6f, 1f, 0.6f);

    private entity_status status;
    private SkillManager skillManager;
    private bool sedangChanneling = false;
    private float channelTimer = 0f;
    private float cooldownTersisa;
    private float manaPerSecond;

    [Header("VFX")]
    public GameObject healVFX;

    private void Awake()
    {
        status = GetComponent<entity_status>();
        skillManager = GetComponent<SkillManager>();
        status.OnDamaged += BatalkanHeal;
        manaPerSecond = manaCost / castTime;
    }

    private void OnDestroy() => status.OnDamaged -= BatalkanHeal;

    private void Update()
    {
        if (cooldownTersisa > 0f) cooldownTersisa -= Time.deltaTime;

        bool tombolDitekan = Input.GetKey(key);

        if (!sedangChanneling)
        {
            if (Input.GetKeyDown(key))
                CobaMulaiHeal();
            return;
        }

        if (!tombolDitekan)
        {
            BatalkanHeal();
            return;
        }

        float dariMana = manaPerSecond * Time.deltaTime;
        if (!status.SpendMana(dariMana))
        {
            BatalkanHeal();
            return;
        }

        channelTimer += Time.deltaTime;

        if (channelTimer >= castTime)
        {
            status.RestoreHealth(healAmount);
            SelesaiHeal();
        }
    }

    private void CobaMulaiHeal()
    {
        if (skillManager.SedangCasting) return;
        if (cooldownTersisa > 0f) return;
        if (status.mana < manaCost)
        {
            Debug.Log("Mana tidak cukup buat Heal");
            return;
        }

        sedangChanneling = true;
        channelTimer = 0f;
        status.SetRooted(true);
        status.SetRegenTint(true, regenTintColor);

        if (healVFX != null)
        {
            healVFX.SetActive(true);
        }
    }

    private void SelesaiHeal()
    {
        sedangChanneling = false;
        channelTimer = 0f;
        cooldownTersisa = cooldown;
        status.SetRooted(false);
        status.SetRegenTint(false, Color.white);
        if (healVFX != null)
        {
            healVFX.SetActive(false);
        }
    }

    private void BatalkanHeal()
    {
        if (!sedangChanneling) return;
        sedangChanneling = false;
        channelTimer = 0f;
        status.SetRooted(false);
        status.SetRegenTint(false, Color.white);
        Debug.Log("Heal batal, mana yang udah kepake hangus");
        if (healVFX != null)
        {
            healVFX.SetActive(false);
        }
    }
}