using UnityEngine;


public class PlayerSneak : MonoBehaviour
{
    [Header("Sneak Settings")]
    [SerializeField] private float hideDuration = 20f;
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private KeyCode sneakKey = KeyCode.CapsLock;

    private entity_status status;
    private PlayerAttack playerAttack;
    private SkillManager skillManager;

    public bool IsHiding { get; private set; }
    public bool IsOnCooldown { get; private set; }
    public float CooldownRemaining => cooldownTimer;

    private float hideTimer;
    private float cooldownTimer;

    public event System.Action<bool> OnHideStateChanged;

    private void Awake()
    {
        status = GetComponent<entity_status>();
        playerAttack = GetComponent<PlayerAttack>();
        skillManager = GetComponent<SkillManager>();
    }

    private void OnEnable()
    {
        if (status != null)
            status.OnDamaged += HandleDamaged;
    }

    private void OnDisable()
    {
        if (status != null)
            status.OnDamaged -= HandleDamaged;
    }

    private void Update()
    {
        HandleTimers();
        HandleInput();
    }

    private void HandleTimers()
    {
        if (IsHiding)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f)
                EndHide();
        }
        else if (IsOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                IsOnCooldown = false;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(sneakKey) && CanStartHide())
        {
            StartHide();
        }
    }

    private bool CanStartHide()
    {
        if (IsHiding || IsOnCooldown) return false;
        if (playerAttack != null && playerAttack.isAttacking) return false;
        if (skillManager != null && skillManager.SedangCasting) return false;
        if (status != null && status.IsRooted) return false;
        return true;
    }

    private void StartHide()
    {
        IsHiding = true;
        hideTimer = hideDuration;
        OnHideStateChanged?.Invoke(true);
    }

    private void EndHide()
    {
        IsHiding = false;
        IsOnCooldown = true;
        cooldownTimer = cooldown;
        OnHideStateChanged?.Invoke(false);
    }

    // Dipanggil otomatis lewat entity_status.OnDamaged saat player kena serangan
    private void HandleDamaged()
    {
        if (IsHiding)
            EndHide();
    }
}