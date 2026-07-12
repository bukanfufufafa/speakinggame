using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("Referensi")]
    public Animator animator;
    public Transform firePoint;

    [Header("Skill Elemen Air (fokus dulu, nanti diganti sistem wheel)")]
    public WaterBallSkillData waterBallData;

    [Header("Resource Pemain")]
    public float manaSaatIni = 100f;
    [Header("Resource Pemain")]
    public InputDetection inputSkill;

    public bool SedangCasting { get; private set; } = false;
    private float cooldownWaterBallTersisa = 0f;

    private void Awake()
    {
        inputSkill.OnTap += HandleTapAir;
        inputSkill.OnHoldStart += HandleHoldMulaiAir;
        inputSkill.OnHoldEnd += HandleHoldSelesaiAir;
    }
    private void OnDestroy()
    {
        // Wajib unsubscribe biar gak memory leak / error saat objek dihancurkan
        inputSkill.OnTap -= HandleTapAir;
        inputSkill.OnHoldStart -= HandleHoldMulaiAir;
        inputSkill.OnHoldEnd -= HandleHoldSelesaiAir;
    }
    private void Update()
    {
        if (cooldownWaterBallTersisa > 0f)
            cooldownWaterBallTersisa -= Time.deltaTime;

        
    }

    private void CobaAktivasiWaterBall()
    {
        if (cooldownWaterBallTersisa > 0f)
        {
            Debug.Log("Water Ball masih cooldown: " + cooldownWaterBallTersisa.ToString("F1") + "s");
            return;
        }

        if (manaSaatIni < waterBallData.manaCost)
        {
            Debug.Log("Mana tidak cukup");
            return;
        }

        manaSaatIni -= waterBallData.manaCost;
        cooldownWaterBallTersisa = waterBallData.cooldownTime;

        SedangCasting = true;

        // Cuma trigger animasi. Proyektil BELUM di-spawn di sini.
        animator.SetTrigger(waterBallData.animatorTriggerName);
    }

    // Dipanggil dari ANIMATION EVENT di clip lempar bola air, bukan dari sini
    public void OnAnimationEvent_SpawnWaterBall()
    {
        int arahHadapX = transform.localScale.x >= 0 ? 1 : -1;

        SkillContext context = new SkillContext
        {
            caster = transform,
            firePoint = firePoint,
            arahHadap = new Vector2(arahHadapX, 0)
        };

        waterBallData.Eksekusi(context);
    }
    public void OnAnimationEvent_SelesaiCasting()
    {
        SedangCasting = false;
    }
    private void HandleTapAir()
    {
        Debug.Log("Tap terdeteksi");
        CobaAktivasiWaterBall(); // untuk sekarang, tap = water ball biasa
    }

    private void HandleHoldMulaiAir()
    {
        Debug.Log("Hold mulai — nanti ini trigger Water Jet");
    }

    private void HandleHoldSelesaiAir()
    {
        Debug.Log("Hold selesai");
    }
}