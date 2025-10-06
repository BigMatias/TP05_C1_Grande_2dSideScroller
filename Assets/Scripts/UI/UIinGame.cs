using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIinGame : MonoBehaviour
{
    [SerializeField] UIinGameData inGameDataSo;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameUiPanel;
    [SerializeField] private TextMeshProUGUI pointCounter;
    [SerializeField] private Button optionsBtn;
    private int pointCounterAux = 0;

    private void Awake()
    {
        optionsBtn.onClick.AddListener(OnOptionsBtnClicked);
        UISelectWeapon.chosenGun += UISelectWeapon_chosenGun;
        EnemyController.onEnemyDie += EnemyController_onEnemyDie;
    }
    private void Start()
    {
        optionsBtn.gameObject.SetActive(false);    
    }

    private void OnDestroy()
    {
        optionsBtn.onClick.RemoveListener(OnOptionsBtnClicked);
        UISelectWeapon.chosenGun -= UISelectWeapon_chosenGun;
        EnemyController.onEnemyDie -= EnemyController_onEnemyDie;
    }

    private void UISelectWeapon_chosenGun(string gun)
    {
        optionsBtn.gameObject.SetActive(true);
    }


    private void EnemyController_onEnemyDie()
    {
        pointCounterAux += inGameDataSo.PointsForKill;
        pointCounter.text = pointCounterAux.ToString();
    }

    private void OnOptionsBtnClicked()
    {
        gameManager.PauseGame();
        pauseMenu.gameObject.SetActive(true);
    }
}
