using UnityEngine;

public abstract class SkillDatabase : ScriptableObject
{
    [Header("Umum")]
    public string namaSkill;
    public ElemenSkill elemenSkill;
    public SkillTier skillTier;

    [Header("Penggunaan")]
    public float cooldownTime = 2f;
    public float manaCost = 10f;
    public float baseDamage = 10f;

    [Header("Animasi")]
    public string animatorTriggerName;

    public abstract void Eksekusi(SkillContext context);
    
}
