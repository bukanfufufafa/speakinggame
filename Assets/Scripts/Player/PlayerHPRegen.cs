using System.Collections;
using UnityEngine;

public class PlayerHPRegen : MonoBehaviour
{
    [Header("Setting")]
    public float castTime = 2f;
    public float manaCost = 30f;
    public float healAmount = 40f;
    public float cooldown = 10f;
    public KeyCode key = KeyCode.Alpha1;

    private entity_status status;
    private SkillManager skillManager;
    private Coroutine channelRoutine;
    private float cooldownTersisa;

    private void Awake()
    {
        status = GetComponent<entity_status>();
        skillManager = GetComponent<SkillManager>();
        status.OnDamaged += BatalkanHeal;
    }

    private void OnDestroy() => status.OnDamaged -= BatalkanHeal;

    private void Update()
    {
        if (cooldownTersisa > 0f) cooldownTersisa -= Time.deltaTime;

        if (Input.GetKeyDown(key) && channelRoutine == null)
            CobaHeal();

        // tombol dilepas sebelum channel selesai -> batal, mana hangus
        if (channelRoutine != null && !Input.GetKey(key))
            BatalkanHeal();
    }

    private void CobaHeal()
    {
        if (skillManager.SedangCasting) return;
        if (cooldownTersisa > 0f) return;
        if (status.mana < manaCost)
        {
            Debug.Log("Mana tidak cukup buat Heal");
            return;
        }

        channelRoutine = StartCoroutine(ChannelHeal());
    }

    private IEnumerator ChannelHeal()
    {
        status.SpendMana(manaCost);
        status.SetRooted(true);
        cooldownTersisa = cooldown;

        float timer = 0f;
        while (timer < castTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        status.RestoreHealth(healAmount);
        status.SetRooted(false);
        channelRoutine = null;
    }

    private void BatalkanHeal()
    {
        if (channelRoutine == null) return;
        StopCoroutine(channelRoutine);
        status.SetRooted(false);
        channelRoutine = null;
        Debug.Log("Heal batal");
    }
}