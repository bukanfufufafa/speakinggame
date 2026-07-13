using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("Referensi")]
    public Animator animator;
    public Transform firePoint;

    [Header("Input")]
    public InputDetection inputSkillE;
    public InputDetection inputSkillQ;

    [Header("Skill Elemen Air")]
    public ProjectileSkillData waterBallData;
    public ProjectileSkillData waterSlashData;

    [Header("Resource Pemain")]
    [SerializeField] private PlayerHealth PlayerHealth;
    //public float manaSaatIni = 100f;

    public bool SedangCasting { get; private set; } = false;

    private Dictionary<SkillDatabase, float> cooldownTersisa = new Dictionary<SkillDatabase, float>();
    private ProjectileSkillData skillYangSedangDicasting; // dipakai animation event

    private void Start()
    {
        PlayerHealth = GetComponent<PlayerHealth>();

    }
    private void Awake()
    {
        inputSkillE.OnTap += () => CobaAktivasiSkill(waterBallData);
        inputSkillQ.OnTap += () => CobaAktivasiSkill(waterSlashData);
    }

    private void Update()
    {
        // Kurangi semua cooldown yang lagi jalan
        List<SkillDatabase> keys = new List<SkillDatabase>(cooldownTersisa.Keys);
        foreach (var skill in keys)
        {
            if (cooldownTersisa[skill] > 0f)
                cooldownTersisa[skill] -= Time.deltaTime;
        }
    }

    private float GetCooldown(SkillDatabase data)
    {
        return cooldownTersisa.TryGetValue(data, out float sisa) ? sisa : 0f;
    }

    private void CobaAktivasiSkill(ProjectileSkillData data)
    {
        if (SedangCasting) return;
        if (GetCooldown(data) > 0f)
        {
            Debug.Log(data.namaSkill + " masih cooldown: " + GetCooldown(data).ToString("F1") + "s");
            return;
        }
        if (PlayerHealth.mana < data.manaCost)
        {
            Debug.Log("Mana tidak cukup");
            return;
        }

        PlayerHealth.mana -= data.manaCost;
        cooldownTersisa[data] = data.cooldownTime;
        SedangCasting = true;
        skillYangSedangDicasting = data;

        animator.SetTrigger(data.animatorTriggerName);
    }

    // SATU fungsi ini dipanggil dari Animation Event skill APAPUN yang tipe proyektil
    public void OnAnimationEvent_SpawnProjectile()
    {
        int arahHadapX = transform.localScale.x >= 0 ? 1 : -1;
        SkillContext context = new SkillContext
        {
            caster = transform,
            firePoint = firePoint,
            arahHadap = new Vector2(arahHadapX, 0)
        };
        skillYangSedangDicasting.Eksekusi(context);
    }

    public void OnAnimationEvent_SelesaiCasting()
    {
        SedangCasting = false;
    }
}