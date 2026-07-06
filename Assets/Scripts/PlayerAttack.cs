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

    private PlayerController2D playerControll;

    private int comboStep = 0;
    private float comboTimer;
    private bool isCooldown = false;
    public bool isAttacking { get; private set; }

    void Start()
    {
        playerControll = GetComponent<PlayerController2D>();
        hitbox.SetActive(false);
    }

    void Update()
    {
        // Reset combo jika terlalu lama
        if (comboTimer > 0)
            comboTimer -= Time.deltaTime;
        else
            comboStep = 0;

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (isCooldown)
            return;


        CancelInvoke(nameof(EndAttackLock));
        CancelInvoke(nameof(DisableHitbox));

        isAttacking = true;

        hitbox.GetComponent<AttakHitBox>().damage = comboDamage[comboStep];

        int dir = playerControll.GetDirection();
        attackPoint.localPosition = new Vector3(dir * attackDistance, 0f, 0f);

        hitbox.SetActive(true);

        anim.SetTrigger("Attack");
        anim.SetInteger("comboStep", comboStep + 1);


        Invoke(nameof(DisableHitbox), hitBoxTime[comboStep]);

        comboStep++;
        comboTimer = comboJeda;

        if (comboStep >= comboDamage.Length)
        {
            isCooldown = true;
            Invoke(nameof(ResetCooldown), cooldownTime);
        }
    }
    void EndAttackLock()
    {
        isAttacking = false;
    }
    void DisableHitbox()
    {
        hitbox.SetActive(false);
    }

    void ResetCooldown()
    {
        isCooldown = false;
        comboStep = 0;
    }
}
