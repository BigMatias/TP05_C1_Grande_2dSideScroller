using UnityEngine;

[CreateAssetMenu(fileName = "BulletSettings", menuName = "Bullet/Data", order = 1)]

public class BulletData : ScriptableObject
{
    [Header("Guns Config")]

    [Header("Damage")]
    public int pistolDamage;
    public int arDamage;
    public int shotgunDamage;

    [Header("Cooldowns")]
    public float arShootingCd;
    public float pistolShootingCd;
    public float shotgunShootingCd;

    [Header("Range")]
    public float arBulletLifeTime;
    public float pistolBulletLifeTime;
    public float shotgunBulletLifeTime;

    [Header("Bullet Speed")]
    public int speed;

    [Header("Shotgun Config")]
    public float shotgunAngleSpread;
    public int shotgunPellets;
}
