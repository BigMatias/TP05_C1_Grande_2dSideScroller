using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] EnemySpawnData enemySpawnDataSo;
    [SerializeField] Transform playerTransform;

    [Header("Spawns")]
    [SerializeField] GameObject[] groundSpawns;
    [SerializeField] GameObject[] airSpawns;

    private GameObject[] enemyCommons;
    private GameObject[] enemyFliers;

    private float enemyTimeSpawn = 0;
    private float enemyCurrentRandomSpawn = 0f;

    void Start()
    {
        enemyCommons = new GameObject[5];
        enemyFliers = new GameObject[5];

        for (int i = 0; i <= enemyCommons.Length - 1; i++)
        {
            GameObject newGameObject = Instantiate(enemySpawnDataSo.Enemies[0], transform);
            newGameObject.name = "EnemyCommon" + i;
            enemyCommons[i] = newGameObject;
            enemyCommons[i].SetActive(false);
        }
        for (int i = 0; i <= enemyFliers.Length - 1; i++)
        {
            GameObject newGameObject = Instantiate(enemySpawnDataSo.Enemies[1], transform);
            newGameObject.name = "EnemyFlier" + i;
            enemyFliers[i] = newGameObject;
            enemyFliers[i].SetActive(false);
        }
    }

    void Update()
    {
        // Spawner
        enemyTimeSpawn += Time.deltaTime;
        if (enemyTimeSpawn > enemyCurrentRandomSpawn)
        {
            enemyCurrentRandomSpawn = UnityEngine.Random.Range(enemySpawnDataSo.EnemyMinRandomSpawn, enemySpawnDataSo.EnemyMaxRandomSpawn);
            enemyTimeSpawn = 0;
            SetEnemyActive();
        }
    }

    private void FixedUpdate()
    {
        MoveSpawnedEnemy();
    }

    private void SetEnemyActive()
    {
        float choice = UnityEngine.Random.Range(0f, 2f);
        if (choice >= 0 && choice <= 1)
        {
            CheckForExistingEnemies(enemyCommons, groundSpawns);
        }
        if (choice >= 1.1 && choice <= 2)
        {
            CheckForExistingEnemies(enemyFliers, airSpawns);   
        }
    }

    private void CheckForExistingEnemies(GameObject[] gameObjectArr, GameObject[] spawns)
    {
        for (int i = 0; gameObjectArr.Length - 1 >= i; i++)
        {
            if (gameObjectArr[i].activeSelf == false)
            {
                float randomSpawn = UnityEngine.Random.Range(0f, 2f);
                if (randomSpawn >= 0 && randomSpawn <= 1)
                {
                    gameObjectArr[i].transform.position = new Vector3(spawns[0].transform.position.x, spawns[0].transform.position.y, 0f);
                }
                else
                {
                    gameObjectArr[i].transform.position = new Vector3(spawns[1].transform.position.x, spawns[1].transform.position.y, 0f);
                }
                gameObjectArr[i].GetComponent<HealthSystem>().ResetLife();
                gameObjectArr[i].SetActive(true);
                break;
            }
        }
    }

    private void MoveSpawnedEnemy()
    {
        for (int i = 0; i <= enemyCommons.Length - 1; i++)
        {
            if (enemyCommons[i].activeSelf)
            {
                GameObject enemy = enemyCommons[i];
                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                float direction = Mathf.Sign(playerTransform.position.x - enemy.transform.position.x);
                rb.velocity = new Vector2(direction * enemySpawnDataSo.SpawnerSpeed, rb.velocity.y);


               /* if (direction > 0)
                    enemy.transform.localScale = new Vector3(-enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z); 
                else
                    enemy.transform.localScale = new Vector3(-enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z); 
               */
            }
        }
        for (int i = 0; i <= enemyFliers.Length - 1; i++)
        {
            if (enemyFliers[i].activeSelf)
            {
                GameObject enemy = enemyFliers[i];
                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                Vector2 newPosition = Vector2.MoveTowards(enemy.transform.position, playerTransform.position, enemySpawnDataSo.SpawnerSpeed * Time.fixedDeltaTime); ;
                rb.MovePosition(newPosition);
            }
        }

    }
}
