using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] enemyAudios;
    [SerializeField] private AudioClip[] music;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceSfx;

    private void Awake()
    {
        PlayerMovement.playerDied += PlayerMovement_playerDied;
        EnemyController.onEnemyDie += EnemyController_onEnemyDie;
    }

    private void OnDestroy()
    {
        PlayerMovement.playerDied -= PlayerMovement_playerDied;
        EnemyController.onEnemyDie -= EnemyController_onEnemyDie;
    }
    private void PlayerMovement_playerDied()
    {
        audioSourceMusic.Stop();
        audioSourceMusic.PlayOneShot(music[1]);
    }

    private void EnemyController_onEnemyDie()
    {
        audioSourceSfx.PlayOneShot(enemyAudios[0]);
    }
}
