using System;
using UnityEngine;
using UnityEngine.UI;

public class UISelectWeapon : MonoBehaviour
{
    public static event Action<string> chosenGun;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Button handgunBtn;
    [SerializeField] private Button arBtn;
    [SerializeField] private Button shotgunBtn;

    private void Awake()
    {        
        arBtn.onClick.AddListener(OnArBtnClicked);
        handgunBtn.onClick.AddListener(OnHandgunBtnClicked);
        shotgunBtn.onClick.AddListener(OnShotgunBtnClicked);
    }

    void Start()
    {
        gameObject.SetActive(true);
        gameManager.PauseGame();
    }

    private void OnDestroy()
    {
        arBtn.onClick.RemoveListener(OnArBtnClicked);
        handgunBtn.onClick.RemoveListener(OnHandgunBtnClicked);
        shotgunBtn.onClick.RemoveListener(OnShotgunBtnClicked);
    }

    private void OnArBtnClicked()
    {
        chosenGun?.Invoke("Ar");
        gameObject.SetActive(false);
        gameManager.PauseGame();
    }

    private void OnHandgunBtnClicked()
    {
        chosenGun?.Invoke("Handgun");
        gameObject.SetActive(false);
        gameManager.PauseGame();
    }

    private void OnShotgunBtnClicked()
    {
        chosenGun?.Invoke("Shotgun");
        gameObject.SetActive(false);
        gameManager.PauseGame();
    }
}
