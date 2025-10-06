using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject OptionsMenu;

    [Header("Music")]
    //Usar array
    [SerializeField] private AudioClip music1;
    [SerializeField] private AudioClip music2;
    [SerializeField] private AudioClip music3;
    [SerializeField] private AudioClip music4;

    [Header("Backgrounds")]
    [SerializeField] private GameObject BG1;
    [SerializeField] private GameObject BG2;
    [SerializeField] private GameObject BG3;
    [SerializeField] private GameObject BG4;

    private bool gamePaused = false;
    private AudioSource audioSource;

    private void Awake()
    {
        Time.timeScale = 1f;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
      /*  int randomMusic = Random.Range(0, 4);
        if (randomMusic == 0)
        {
            audioSource.clip = music1;
            audioSource.Play();
           
        }
        else if (randomMusic == 1)
        {
            audioSource.clip = music2;
            audioSource.Play();
        }
        else if (randomMusic == 2)
        {
            audioSource.clip = music3;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = music4;
            audioSource.Play();
        }

        int randomBG = Random.Range(0, 4);
        if (randomBG == 0)
        {
            BG1.gameObject.SetActive(true);
        }
        else if (randomBG == 1)
        {
            BG2.gameObject.SetActive(true);
        }
        else if (randomBG == 2)
        {
            BG3.gameObject.SetActive(true);
        }
        else
        {
            BG4.gameObject.SetActive(true);
        }*/
     
    }

    void Update()
    {
        audioSource.loop = true;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
            {
                PauseGame();
                PauseMenu.gameObject.SetActive(true);
                OptionsMenu.gameObject.SetActive(false);
            }
            else
            {
                PauseGame();
                PauseMenu.gameObject.SetActive(false);
                OptionsMenu.gameObject.SetActive(false);
            }
        }
    }

    public void PauseGame()
    {
        if (!gamePaused)
        {
            Time.timeScale = 0f;
            gamePaused = true;
            
        }
        else
        {
            Time.timeScale = 1f;
            gamePaused = false;
        }
    }

}
