using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Setup")]
    public Transform attackPoint;
    // attackDistance dihapus karena posisi akan mengikuti settingan manual di Inspector

    [Header("Efek Visual (Circle Sihir)")]
    public GameObject magicCirclePrefab;
    public float magicCircleDuration = 1f; // Waktu sebelum circle sihir hancur otomatis

    [Header("Combo - Proyektil")]
    public GameObject[] comboProjectilePrefab = new GameObject[3];
    public float[] comboProjectileSpeed = { 10f, 10f, 10f };

    [Header("Combo Stats")]
    public int[] comboDamage = { 10, 15, 25 };

    [Tooltip("Waktu jeda (detik) bagi player untuk menekan tombol agar lanjut combo setelah animasi selesai")]
    public float comboJeda = 0.8f;

    [Header("Animator")]
    public Animator anim;

    [Header("Cooldown")]
    public float cooldownTime = 0.5f;

    private int comboStep = 0;
    private int comboStepAktif = 0;
    private float comboTimer;
    private bool isCooldown = false;
    private bool inputBuffered = false;
    private PlayerController2D playerControll;
    private characterStat stats;
    private float timerSetFalse;//timer buat sset false attack
    public float batasWaktuAnimasi = 1.5f;
    public bool isAttacking { get; private set; }

    void Start()
    {
        playerControll = GetComponent<PlayerController2D>();
        stats = GetComponent<characterStat>();
    }

    void Update()
    {
        if (isAttacking)
        {// attacknya di anuin kalau error nanti di reset
            timerSetFalse -= Time.deltaTime;
            if (timerSetFalse <= 0)
            {
                Debug.LogWarning("Karakter Di reset.");
                isAttacking = false;
                inputBuffered = false;
                isCooldown = false;
                comboStep = 0;
                anim.SetInteger("comboStep", 0);
            }
        }
        else
        {
            if (comboTimer > 0)
                comboTimer -= Time.deltaTime;
            else
            {
                comboStep = 0;
                anim.SetInteger("comboStep", 0);
            }
        }

        bool klikSerangan = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F);

        if (klikSerangan)
        {
            if (!isCooldown && !isAttacking)
                Attack();
            else if (isAttacking && !isCooldown)
                inputBuffered = true; // Buffer input untuk serangan berikutnya
        }
    }

    void Attack()
    {
        isAttacking = true;
        comboStepAktif = comboStep;
        timerSetFalse = batasWaktuAnimasi;


        anim.SetTrigger("Attack");
        anim.SetInteger("comboStep", comboStep + 1);

        comboStep++;

        // Jika mencapai serangan terakhir di array combo, siapkan cooldown
        if (comboStep >= comboDamage.Length)
        {
            isCooldown = true;
        }
    }

    // Dipanggil dari Animation Event, di frame saat karakter melempar sihir
    public void OnAnimationEvent_SpawnComboProjectile()
    {
        if (magicCirclePrefab != null)
        {
            GameObject circle = Instantiate(magicCirclePrefab, attackPoint.position, Quaternion.identity);
            Destroy(circle, magicCircleDuration); // Hancurkan setelah durasi selesai
        }

        GameObject prefab = comboProjectilePrefab[comboStepAktif];
        if (prefab == null)
        {
            Debug.LogWarning("Prefab proyektil combo step " + comboStepAktif + " belum diisi di Inspector");
            return;
        }

        int finalDamage = comboDamage[comboStepAktif];
        if (stats != null)
            finalDamage += (stats.Intelligence / 2);

        int dir = playerControll.GetDirection();

        GameObject peluru = Instantiate(prefab, attackPoint.position, Quaternion.identity);
        WaterBallProjectile skripPeluru = peluru.GetComponent<WaterBallProjectile>();

        if (skripPeluru != null)
        {
            skripPeluru.Inisialisasi(new Vector2(dir, 0), comboProjectileSpeed[comboStepAktif], finalDamage);

            if (dir < 0)
            {
                peluru.transform.localScale = new Vector3(-Mathf.Abs(peluru.transform.localScale.x), peluru.transform.localScale.y, peluru.transform.localScale.z);
            }
        }
    }

    public void OnAnimationEvent_EndAttack()
    {
        Debug.Log("Event EndAttack Terpanggil! isAttacking sekarang false.");
        isAttacking = false;

        // Mulai hitung waktu jeda (window untuk lanjut combo) SETELAH animasi selesai sepenuhnya
        comboTimer = comboJeda;
        anim.ResetTrigger("Attack");

        if (inputBuffered && !isCooldown)
        {
            inputBuffered = false;
            Attack(); // Lanjut ke combo berikutnya
        }
        else
        {
            inputBuffered = false;
            if (isCooldown)
            {
                Invoke(nameof(ResetCooldown), cooldownTime);
            }
        }
    }

    void ResetCooldown()
    {
        isCooldown = false;
        comboStep = 0;
        anim.SetInteger("comboStep", 0);
    }
}