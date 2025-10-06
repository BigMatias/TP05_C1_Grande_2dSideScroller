using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private Rigidbody2D rb;

    private void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Set (int speed, int parmDamage)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = transform.right * speed ;
        damage = parmDamage;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.TryGetComponent(out HealthSystem healthSystem))
        {
            Destroy(gameObject);
            healthSystem.DoDamage(damage);
        }
        if (other.gameObject.layer == (int)LayersEnum.Layers.Floor || other.gameObject.layer == (int)LayersEnum.Layers.Walls)
        {
            Destroy(gameObject);
        }
    }
}