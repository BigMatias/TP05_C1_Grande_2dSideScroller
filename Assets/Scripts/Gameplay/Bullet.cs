using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletData bulletDataSo;

    private Rigidbody2D rb;
    private int damage;


    private void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        switch (this.name)
        {
            case "ArBullet(Clone)":
               StartCoroutine(LifeTimer(bulletDataSo.arBulletLifeTime));
                break;

            case "HandgunBullet(Clone)":
                StartCoroutine(LifeTimer(bulletDataSo.pistolBulletLifeTime));
                break;

            case "ShotgunBullet(Clone)":
                StartCoroutine(LifeTimer(bulletDataSo.shotgunBulletLifeTime));
                break;

        }
    }

    private IEnumerator LifeTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.TryGetComponent(out HealthSystem healthSystem))
        {
            gameObject.SetActive(false);
            healthSystem.DoDamage(damage);
        }
    }

    public void Set (int speed, int parmDamage)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = transform.right * speed ;
        damage = parmDamage;
    }



}