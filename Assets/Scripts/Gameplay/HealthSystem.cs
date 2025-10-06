using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxLife = 100;

    public event Action<int, int> onLifeUpdated; // <currentLife, maxLife>
    public event Action onDie;

    private int life = 100;

    private void Start()
    {
        life = maxLife;
        onLifeUpdated?.Invoke(life, maxLife);
    }

    public void ResetLife()
    {
        life = maxLife;
        onLifeUpdated?.Invoke(life, maxLife);
    }

    public void DoDamage(int damage)
    {
        if (damage < 0)
        {
            Debug.Log("Se cura en la funcion de daño");
            return;
        }

        life -= damage;

        if (life <= 0)
        {
            life = 0;
            onDie?.Invoke();
        }
        else
        {
            onLifeUpdated?.Invoke(life, maxLife);
        }

    }

    public void Heal(int plus)
    {
        if (plus < 0)
        {
            return;
        }

        life += plus;

        if (life > maxLife)
            life = maxLife;

        onLifeUpdated?.Invoke(life, maxLife);
    }
}