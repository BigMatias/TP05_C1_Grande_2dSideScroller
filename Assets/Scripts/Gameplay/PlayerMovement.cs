using System;
using System.Collections;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerData PlayerData;
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioClip dead;
    [SerializeField] private AudioClip coinPickedUp;
    [SerializeField] private AudioClip goldPowerUp;

    [NonSerialized] public bool potionPickedUp = false;
    [NonSerialized] public bool goldPowerUpPickedUp = false;

    //private static readonly int State = Animator.StringToHash("State");
    private bool grounded = false;

    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        //animator.SetInteger(State, (int)playerState);
        rb.gravityScale = 5f;
    }
    private void Update()
    {
        Fall();
        if (Input.GetMouseButton(0))
        {
            Fire();
        }
    }

    private void FixedUpdate()
    {
        if (grounded)
        {
            // playerState = PlayerState.Run;
            // animator.SetInteger(State, (int)playerState);
        }
        MovePlayer();
    }


    private void Fall()
    {
        if (Input.GetKeyUp(PlayerData.Jump))
        {
            rb.gravityScale = PlayerData.GravityScaleFall;
        }

        if (rb.velocity.y <= 0f && !grounded)
        {
           // playerState = PlayerState.Fall;
           // animator.SetInteger(State, (int)playerState);
        }
    }

    private void MovePlayer()
    {
        Vector2 velocity = rb.velocity;

        if (Input.GetKey(PlayerData.Left))
        {
            velocity.x = -PlayerData.Speed;
        }
        else if (Input.GetKey(PlayerData.Right))
        {
            velocity.x = PlayerData.Speed;
        }
        else
        {
            velocity.x = 0f;
        }

        if (Input.GetKey(PlayerData.Jump) && grounded)
        {
            // playerState = PlayerState.Jump;
            // animator.SetInteger(State, (int)playerState);
            rb.AddForce(PlayerData.JumpSpeed * Time.fixedDeltaTime * Vector2.up);
        }

        rb.velocity = velocity;
    }

    void Fire()
    {
       // if (EventSystem.current.IsPointerOverGameObject())
         //   return;

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //bullet.transform.LookAt(targetPos)
        //bullet.Set(20, 30)
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            rb.gravityScale = PlayerData.GravityScaleJump;
            grounded = true;
            // animator.SetInteger(State, (int)playerState);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            //playerState = PlayerState.Die;
            //animator.SetInteger(State, (int)playerState);
            rb.gravityScale = PlayerData.GravityScaleDead;
            //audioSource.PlayOneShot(dead);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            //audioSource.Play();
            StartCoroutine(PotionPickedUp());
        }
        if (collision.gameObject.layer == 11)
        {
            //audioSource.PlayOneShot(coinPickedUp);
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.layer == 12)
        {
            //audioSource.PlayOneShot(goldPowerUp);
            collision.gameObject.SetActive(false);
            StartCoroutine(GoldPowerUpPickedUp());
        }
    }

    private IEnumerator PotionPickedUp()
    {
        potionPickedUp = true;
        yield return new WaitForSeconds(10);
        potionPickedUp = false;

    }
    private IEnumerator GoldPowerUpPickedUp()
    {
        goldPowerUpPickedUp = true;
        yield return new WaitForSeconds(10);
        goldPowerUpPickedUp = false;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            grounded = false;
        }
    }
}
