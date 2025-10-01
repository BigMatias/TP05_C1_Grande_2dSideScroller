using UnityEngine;

public class Bullet: MonoBehaviour
{
    [SerializeField] private BulletData bulletData;

    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Set(int speed, int damage)
    {
        rigidbody2D.bodyType  = RigidbodyType2D.Dynamic;
        rigidbody2D.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

}


