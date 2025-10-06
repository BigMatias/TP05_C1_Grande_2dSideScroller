using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnSettings", menuName = "EnemySpawn/Data")]

public class EnemySpawnData : ScriptableObject
{
    public GameObject[] Enemies;

    [Header("Config")]
    public float EnemyMinRandomSpawn = 2f;
    public float EnemyMaxRandomSpawn = 4f;
    public float SpawnerSpeed = 1f;
}
