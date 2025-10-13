using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyData enemyDataSo;
    [SerializeField] private ParticleSystem enemyDeathParticles;

    public static event Action onEnemyDie;
    private HealthSystem healthSystem;

    private void Awake ()
    {
        healthSystem = GetComponent<HealthSystem>();   
        healthSystem.onDie += HealthSystem_onDie;
    }

    private void HealthSystem_onDie()
    {
        Instantiate(enemyDeathParticles, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        onEnemyDie?.Invoke();

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out HealthSystem healthSystem))
        {
            healthSystem.DoDamage(enemyDataSo.EnemyDamage);
        }
    }



}
