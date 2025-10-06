using UnityEngine;

[CreateAssetMenu(fileName = "BulletSettings", menuName = "Bullet/Data", order = 1)]

public class BulletData : ScriptableObject
{
    public int damage;
    public int speed;
}
