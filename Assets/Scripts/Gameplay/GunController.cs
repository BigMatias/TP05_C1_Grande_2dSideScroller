using System.Collections;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

public class GunController : MonoBehaviour
{
    [SerializeField] private BulletData bulletDataSo;
    [SerializeField] private GameObject[] faceSprites;
    [SerializeField] private GunData gunDataSo;

    private AudioSource audioSource;
    private Transform aimTransform;
    private GameObject currentGun;
    private GameObject muzzleFlash;
    private Transform gunFirePoint;
    private Bullet currentBullet;
    private Bullet[] bullets;

    private int chosenGun;
    private bool isGunInCd = false;
    private Vector3 originalLocalPos;
    private Vector3 targetLocalPos;

    private float currentRecoilDistance;
    private float currentRecoilSpeed;
    private bool isPlaying;

    private void Awake()
    {
        UISelectWeapon.chosenGun += UISelectWeapon_chosenGun;
        aimTransform = transform.Find("Aim");
    }

    private void OnDestroy()
    {
        UISelectWeapon.chosenGun -= UISelectWeapon_chosenGun;
    }

    private void Start()
    {
        bullets = new Bullet[50];
    }

    private void Update()
    {
        if (Time.timeScale == 1f)
        {
            HandleAiming();
            HandleShooting();
        }
        if (chosenGun != 0)
        {
            currentGun.transform.localPosition = Vector3.Lerp(
            currentGun.transform.localPosition,
            targetLocalPos,

            Time.deltaTime * currentRecoilSpeed);
        }
    }

    private void UISelectWeapon_chosenGun(string tempChosenGun)
    {
        switch (tempChosenGun)
        {
            case "Ar":
                currentGun = Instantiate(gunDataSo.gunsPrefabs[0], aimTransform);
                currentBullet = gunDataSo.bulletPrefab[0];
                currentRecoilDistance = gunDataSo.arRecoilDistance;
                currentRecoilSpeed = gunDataSo.arRecoilSpeed;

                chosenGun = 1;
                break;

            case "Handgun":
                currentGun = Instantiate(gunDataSo.gunsPrefabs[1], aimTransform);
                currentBullet = gunDataSo.bulletPrefab[1];
                currentRecoilDistance = gunDataSo.handgunRecoilDistance;
                currentRecoilSpeed = gunDataSo.handgunRecoilSpeed;
                chosenGun = 2;
                break;

            case "Shotgun":
                currentGun = Instantiate(gunDataSo.gunsPrefabs[2], aimTransform);
                currentBullet = gunDataSo.bulletPrefab[2];
                currentRecoilDistance = gunDataSo.shotgunRecoilDistance;
                currentRecoilSpeed = gunDataSo.shotgunRecoilSpeed;
                chosenGun = 3;
                break;
        }
        audioSource = currentGun.GetComponent<AudioSource>();
        gunFirePoint = currentGun.transform.Find("Fire Point");
        muzzleFlash = currentGun.transform.Find("Muzzle Flash").gameObject;
        originalLocalPos = currentGun.transform.localPosition;
        targetLocalPos = originalLocalPos;
        CreateBulletPool();
    }

    private void CreateBulletPool()
    {
        for (int i = 0; i <= bullets.Length - 1; i++)
        {
            bullets[i] = Instantiate(currentBullet, transform.Find("Bullets"));
            bullets[i].gameObject.SetActive(false);
        }

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
        switch (chosenGun)
        {
            case 1: //Ar
                if (Input.GetMouseButtonDown(0) && !isGunInCd)
                {
                    audioSource.PlayOneShot(gunDataSo.arSounds[0]);
                }

                if (Input.GetMouseButton(0) && !isGunInCd)
                {
                    if (EventSystem.current.IsPointerOverGameObject()) // Para saber si le pego a un objeto de UI
                        return;
                    for (int i = 0; i <= bullets.Length; i++)
                    {
                        if (!bullets[i].gameObject.activeSelf)
                        {
                            Bullet bullet = bullets[i];
                            bullet.gameObject.SetActive(true);
                            bullet.transform.position = gunFirePoint.position;
                            bullet.transform.rotation = gunFirePoint.rotation;
                            bullet.gameObject.layer = LayerMask.NameToLayer("Bullet");
                            bullet.Set(bulletDataSo.speed, bulletDataSo.arDamage);
                            StartArSoundLoop();

                            AnimationsAndSound();
                            StartCoroutine(GunCooldown(bulletDataSo.arShootingCd));
                            break;
                        }
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    StopArSoundLoop();
                    audioSource.PlayOneShot(gunDataSo.arSounds[2]);
                }

                break;

            case 2: //Handgun
                if (Input.GetMouseButton(0) && !isGunInCd)
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                        return;

                    for (int i = 0; i <= bullets.Length; i++)
                    {
                        if (!bullets[i].gameObject.activeSelf)
                        {
                            Bullet bullet = bullets[i];
                            bullet.gameObject.SetActive(true);
                            bullet.transform.position = gunFirePoint.position;
                            bullet.transform.rotation = gunFirePoint.rotation;
                            bullet.gameObject.layer = LayerMask.NameToLayer("Bullet");
                            bullet.Set(bulletDataSo.speed, bulletDataSo.pistolDamage);

                            AnimationsAndSound();
                            StartCoroutine(GunCooldown(bulletDataSo.pistolShootingCd));
                            break;
                        }
                    }
                }
                break;

            case 3: //Shotgun
                if (Input.GetMouseButton(0) && !isGunInCd)
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                        return;

                    for (int i = 0; i < bullets.Length - bulletDataSo.shotgunPellets; i++)
                    {
                        if (!bullets[i].gameObject.activeSelf)
                        {
                            for (int j = 0; j < bulletDataSo.shotgunPellets; j++)
                            {
                                Bullet bullet = bullets[i + j];
                                bullet.gameObject.SetActive(true);
                                bullet.transform.position = gunFirePoint.position;
                                bullet.gameObject.layer = LayerMask.NameToLayer("Bullet");

                                float angleOffset = Mathf.Lerp(-bulletDataSo.shotgunAngleSpread, bulletDataSo.shotgunAngleSpread, j / (float)(bulletDataSo.shotgunPellets - 1));
                                Quaternion spreadRotation = gunFirePoint.rotation * Quaternion.Euler(0f, 0f, angleOffset);
                                bullet.transform.rotation = spreadRotation;
                                bullet.Set(bulletDataSo.speed, bulletDataSo.shotgunDamage);

                                AnimationsAndSound();
                                StartCoroutine(GunCooldown(bulletDataSo.shotgunShootingCd));
                            }

                            break;
                        }
                    }
                }
                break;
        }
    }

    private void AnimationsAndSound()
    {
        faceSprites[0].SetActive(false);
        faceSprites[1].SetActive(true);
        muzzleFlash.gameObject.SetActive(true);
        if (chosenGun != 1)
        {
            audioSource.Play();
        }

        if (aimTransform.eulerAngles.z >= 50 && aimTransform.eulerAngles.z <= 150)
        {
            targetLocalPos = originalLocalPos - Vector3.down * currentRecoilDistance;
        }
        else if (aimTransform.eulerAngles.z <= -50 && aimTransform.eulerAngles.z >= -150)
        {
            targetLocalPos = originalLocalPos - Vector3.up* currentRecoilDistance;
        }

        targetLocalPos = originalLocalPos - Vector3.right * currentRecoilDistance;

        StopAllCoroutines();
        StartCoroutine(ReturnRecoil());
    }

    private IEnumerator ReturnRecoil()
    {
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.gameObject.SetActive(false);
        targetLocalPos = originalLocalPos;
    }

    private IEnumerator GunCooldown(float cooldown)
    {
        isGunInCd = true;
        yield return new WaitForSeconds(cooldown);
        isGunInCd = false;
        faceSprites[1].SetActive(false);
        faceSprites[0].SetActive(true);
    }

    private void StartArSoundLoop()
    {
        if (isPlaying) return;
        audioSource.loop = true;
        audioSource.clip = gunDataSo.arSounds[1];
        audioSource.Play();
        isPlaying = true;
    }

    private void StopArSoundLoop()
    {
        if (!isPlaying) return;
        audioSource.loop = false;
        audioSource.Stop();
        isPlaying = false;
    }
}
