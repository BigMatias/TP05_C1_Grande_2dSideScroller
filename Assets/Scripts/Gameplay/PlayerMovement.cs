using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerDataSo playerDataSo;
    [SerializeField] private BulletData bulletDataSo;
    [SerializeField] private GameObject[] faceSprites;
    [SerializeField] private AudioClip[] audioClips;

    [Header("Ground check")]
    [SerializeField] private Transform groundCheck;
    private float groundRadius = 0.4f;

    public static event Action playerDied;

    private Rigidbody2D rb;
    private HealthSystem healthSystem;
    private AudioSource audioSource;

    [SerializeField] private LayerMask groundLayer;

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.onDie += HealthSystem_onDie;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        healthSystem.onDie -= HealthSystem_onDie;
    }

    private void Start()
    {
        rb.gravityScale = playerDataSo.GravityScaleJump;
    }

    private void Update()
    {
        Fall();
    }

    private void FixedUpdate()
    {
        MovePlayer();
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
            audioSource.PlayOneShot(audioClips[0]);
            rb.gravityScale = playerDataSo.GravityScaleJump;
            rb.AddForce(playerDataSo.JumpSpeed * Time.fixedDeltaTime * Vector2.up);
        }

        rb.velocity = velocity;
    }

    private void HealthSystem_onDie()
    {
        faceSprites[0].SetActive(false);
        faceSprites[1].SetActive(false);
        faceSprites[2].SetActive(true);

        audioSource.PlayOneShot(audioClips[1]);
        playerDied?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)LayersEnum.Layers.LifePotion)
        {
            healthSystem.Heal(20);
            collision.gameObject.SetActive(false);
            audioSource.PlayOneShot(audioClips[2]);
        }
    }
}
