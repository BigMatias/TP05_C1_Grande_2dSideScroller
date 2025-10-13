using UnityEngine;

[CreateAssetMenu(fileName = "GunSettings", menuName = "Gun/Data")]

public class GunData : ScriptableObject
{
    [Header("References")]
    public Bullet[] bulletPrefab;
    public GameObject[] gunsPrefabs;

    [Header("Configs")]
    public float arRecoilDistance;
    public float arRecoilSpeed;
    public float handgunRecoilDistance;
    public float handgunRecoilSpeed;
    public float shotgunRecoilDistance;
    public float shotgunRecoilSpeed;

    [Header("Sounds")]
    public AudioClip[] arSounds;
}
