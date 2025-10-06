using System;
using System.Collections;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerDataSo playerDataSo;
    [SerializeField] private BulletData bulletDataSo;
    [SerializeField] private AudioClip dead;
    [SerializeField] private AudioClip coinPickedUp;
    [SerializeField] private Animator playerAnimator;

    [Header("Ground check")]
    [SerializeField] private Transform groundCheck;
    private float groundRadius = 0.4f;

    [NonSerialized] public bool potionPickedUp = false;
    [NonSerialized] public bool goldPowerUpPickedUp = false;

    public static event Action playerDied;

    private Rigidbody2D rb;
    private HealthSystem healthSystem;

    private Transform aimTransform;
    private GameObject currentGun;
    private Bullet currentBullet;
    private Transform gunFirePoint;

    private static readonly int State = Animator.StringToHash("State");
    private int chosenGun;

    [SerializeField] private LayerMask groundLayer;

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    enum PlayerState
    {
        Idle,
        Shoot,
        Dead
    }

    [SerializeField] private PlayerState playerState = PlayerState.Idle;

    //public event EventHandler OnShoot;
/*    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
    }
*/
    private void Awake()
    {
        UISelectWeapon.chosenGun += UISelectWeapon_chosenGun;
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.onDie += HealthSystem_onDie;
        aimTransform = transform.Find("Aim");
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        UISelectWeapon.chosenGun -= UISelectWeapon_chosenGun;
        healthSystem.onDie -= HealthSystem_onDie;
    }

    private void Start()
    {
        playerAnimator.SetInteger(State, (int)playerState);
        rb.gravityScale = playerDataSo.GravityScaleJump;
    }

    private void Update()
    {
        if (Time.timeScale == 1f)
        {
            HandleAiming();
            HandleShooting();
        }
        Fall();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0f, 0f, angle);

        if (currentGun != null)
        {
            if (aimTransform.eulerAngles.z >= 90f && aimTransform.eulerAngles.z <= 260f)
            {
                currentGun.transform.localScale = new Vector3(1f, -1f, 1f);
            }
            else
            {
                currentGun.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) // Para saber si le pego a un objeto de UI
            return;
            switch (chosenGun)
            {
                case 1: // Ar
                    FireBullet(1);
                    break;

                case 2: // Handgun
                    FireBullet(2);
                    break;

                case 3: // Shotgun
                    FireBullet(3);
                    break;
            }
        }
    }

    private void FireBullet(int chosenGun)
    {
        Bullet bullet = Instantiate(currentBullet);
        bullet.transform.position = gunFirePoint.position;
        bullet.transform.rotation = gunFirePoint.rotation;
        bullet.gameObject.layer = LayerMask.NameToLayer("Bullet");

        bullet.Set(bulletDataSo.speed, bulletDataSo.damage);
    }

    private void Fall()
    {
        if (Input.GetKeyUp(playerDataSo.Jump))
        {
            rb.gravityScale = playerDataSo.GravityScaleFall;
        }
    }


    private void MovePlayer()
    {
        Vector2 velocity = rb.velocity;

        if (Input.GetKey(playerDataSo.Left))
        {
            velocity.x = -playerDataSo.Speed;
        }
        else if (Input.GetKey(playerDataSo.Right))
        {
            velocity.x = playerDataSo.Speed;
        }
        else
        {
            velocity.x = 0f;
        }

        if (Input.GetKey(playerDataSo.Jump) && IsGrounded())
        {
            rb.gravityScale = playerDataSo.GravityScaleJump;
            rb.AddForce(playerDataSo.JumpSpeed * Time.fixedDeltaTime * Vector2.up);
        }

        rb.velocity = velocity;
    }
    private void HealthSystem_onDie()
    {
        playerDied?.Invoke();
    }

    private void UISelectWeapon_chosenGun(string tempChosenGun)
    {
        switch (tempChosenGun)
        {
            case "Ar":
                currentGun = Instantiate(playerDataSo.gunsPrefabs[0], aimTransform);
                currentBullet = playerDataSo.bulletPrefab[0];
                gunFirePoint = currentGun.transform.Find("Fire Point");
                chosenGun = 1;
                break;

            case "Handgun":
                currentGun = Instantiate(playerDataSo.gunsPrefabs[1], aimTransform);
                currentBullet = playerDataSo.bulletPrefab[1];
                gunFirePoint = currentGun.transform.Find("Fire Point");
                chosenGun = 2;
                break;

            case "Shotgun":
                 currentGun = Instantiate(playerDataSo.gunsPrefabs[2], aimTransform);
                currentBullet = playerDataSo.bulletPrefab[2];
                gunFirePoint = currentGun.transform.Find("Fire Point");
                chosenGun = 3;
                break;
        }
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)LayersEnum.Layers.LifePotion)
        {
            healthSystem.Heal(20);
            collision.gameObject.SetActive(false);
            //audioSource.Play();
        }
    }
}
