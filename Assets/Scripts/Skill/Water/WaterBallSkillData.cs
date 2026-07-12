using UnityEngine;

[CreateAssetMenu(fileName = "WaterBallSkillData", menuName = "Skill/Air/WaterBall")]
public class WaterBallSkillData : SkillDatabase
{
    [Header("Water Ball - Khusus")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;

    public override void Eksekusi(SkillContext context)
    {
        GameObject peluru = Object.Instantiate(projectilePrefab, context.firePoint.position, Quaternion.identity);
        WaterBallProjectile skripPeluru = peluru.GetComponent<WaterBallProjectile>();
        if (skripPeluru != null)
        {
            skripPeluru.Inisialisasi(context.arahHadap, projectileSpeed, baseDamage);
        }
    }
}