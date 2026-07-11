using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Setup")]
    public GameObject hitbox;
    public Transform attackPoint;
    public float attackDistance = 0.7f;

    [Header("Combo")]
    public int[] comboDamage = { 10, 15, 25 };
    public float[] hitBoxTime = { 0.15f, 0.15f, 2f };
    public float comboJeda = 0.4f;

    [Header("Animator")]
    public Animator anim;
    [Header("Cooldown")]
    public float cooldownTime = 0.5f;

    private int comboStep = 0;
    private float comboTimer;
    private bool isCooldown = false;
    private bool inputBuffered = false; // nyimpen klik yang masuk pas masih animasi
    private PlayerController2D playerControll;
    private characterStat stats;
    public bool isAttacking { get; private set; }

    void Start()
    {
        playerControll = GetComponent<PlayerController2D>();
        stats = GetComponent<characterStat>();
        hitbox.SetActive(false);
    }

    void Update()
    {
        if (!isAttacking)
        {
            if (comboTimer > 0)
                comboTimer -= Time.deltaTime;
            else
                comboStep = 0;
        }

        bool klikSerangan = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F);

        if (klikSerangan)
        {
            if (!isCooldown && !isAttacking)
            {
                // klik pas lagi free (tidak nyerang) -> langsung serang
                Attack();
            }
            else if (isAttacking && !isCooldown)
            {
                // klik pas animasi masih jalan -> jangan dibuang, simpan dulu
                inputBuffered = true;
            }
        }
    }

    void Attack()
    {
        CancelInvoke(nameof(EndAttackLock));
        CancelInvoke(nameof(DisableHitbox));

        isAttacking = true;

        int finalDamage = comboDamage[comboStep];
        if (stats != null)
        {
            finalDamage += (stats.Intelligence / 2);
        }

        hitbox.GetComponent<AttakHitBox>().damage = finalDamage;

        int dir = playerControll.GetDirection();
        attackPoint.localPosition = new Vector3(dir * attackDistance, 0f, 0f);

        hitbox.SetActive(true);

        anim.SetTrigger("Attack");
        anim.SetInteger("comboStep", comboStep + 1);

        float currentAttackTime = hitBoxTime[comboStep];

        Invoke(nameof(DisableHitbox), currentAttackTime);
        Invoke(nameof(EndAttackLock), currentAttackTime);
        comboStep++;
        comboTimer = currentAttackTime + comboJeda;

        if (comboStep >= comboDamage.Length)
        {
            isCooldown = true;
            // cooldown mulai dihitung SETELAH attack lock hit terakhir selesai,
            // jadi tidak bentrok lagi kayak sebelumnya
            Invoke(nameof(ResetCooldown), currentAttackTime + cooldownTime);
        }
    }

    void EndAttackLock()
    {
        isAttacking = false;

        // kalau ada klik yang ke-buffer selama animasi tadi, lanjutkan combo otomatis
        if (inputBuffered && !isCooldown)
        {
            inputBuffered = false;
            Attack();
        }
        else
        {
            inputBuffered = false;
        }
    }

    void DisableHitbox()
    {
        hitbox.SetActive(false);
    }

    void ResetCooldown()
    {
        isCooldown = false;
        comboStep = 0;
        inputBuffered = false;
        // isAttacking sengaja tidak disentuh di sini, biar EndAttackLock
        // yang selalu jadi satu-satunya yang ngatur isAttacking
    }
}