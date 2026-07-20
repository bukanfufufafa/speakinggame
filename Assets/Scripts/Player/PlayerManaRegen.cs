using UnityEngine;

public class PlayerManaRegen : MonoBehaviour
{
    [Header("Settings")]
    public float restoreRate = 20f;
    public KeyCode key = KeyCode.Alpha2;
    public Color regenTintColor = new Color(0.6f, 0.85f, 1f);

    [Header("VFX")]
    public GameObject manaVFX;

    private entity_status status;
    private SkillManager skillManager;
    private bool sedangRestore = false;

    private void Awake()
    {
        status = GetComponent<entity_status>();
        skillManager = GetComponent<SkillManager>();

        status.OnDamaged += BatalkanRestore;
    }

    private void OnDestroy()
    {
        if (status != null)
            status.OnDamaged -= BatalkanRestore;
    }

    private void Update()
    {
        bool tombolDitekan = Input.GetKey(key);
        bool cobaGerak = Input.GetKey(KeyCode.A) ||
                         Input.GetKey(KeyCode.D) ||
                         Input.GetButtonDown("Jump");

        bool masihBisaIsi = status.mana < status.maxMana;

        if (sedangRestore && (cobaGerak || !tombolDitekan || !masihBisaIsi))
        {
            BatalkanRestore();
            return;
        }

        if (tombolDitekan &&
            !sedangRestore &&
            !skillManager.SedangCasting &&
            masihBisaIsi)
        {
            sedangRestore = true;

            status.SetRooted(true);
            status.SetRegenTint(true, regenTintColor);

            if (manaVFX != null)
                manaVFX.SetActive(true);
        }

        if (sedangRestore)
        {
            status.RestoreMana(restoreRate * Time.deltaTime);
        }
    }

    private void BatalkanRestore()
    {
        if (!sedangRestore) return;

        sedangRestore = false;

        status.SetRooted(false);
        status.SetRegenTint(false, Color.white);

        if (manaVFX != null)
            manaVFX.SetActive(false);
    }
}