using System.Collections;
using UnityEngine;

public class PickablesSpawner : MonoBehaviour
{
    [SerializeField] private PickablesData pickablesData;
    [SerializeField] private GameObject potionPrefab;
    [SerializeField] private Transform[] pickableSpawns;

    private GameObject potion;
    private float timer;

    void Start()
    {
        potion = Instantiate(potionPrefab, transform);    
        potion.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > pickablesData.LifePotionSpawnTime)
        {
            timer = 0;
            SetPotionActive();
        }
    }

    private void SetPotionActive()
    {
        float randomPos = Random.Range(1f, 3f);
        if (randomPos >= 0 && randomPos <= 1)
        {
            potion.transform.position = pickableSpawns[0].position;
            StartCoroutine(StartDeactivateTimer(potion));
        }
        else if (randomPos >= 1.1 && randomPos <= 2)
        {
            potion.transform.position = pickableSpawns[1].position;
            StartCoroutine(StartDeactivateTimer(potion));
        }
        else if (randomPos >= 2.1 && randomPos <= 3)
        {
            potion.transform.position = pickableSpawns[2].position;
            StartCoroutine(StartDeactivateTimer(potion));
        }
    }

    private IEnumerator StartDeactivateTimer(GameObject pickable)
    {
        pickable.SetActive(true);
        yield return new WaitForSeconds(10);
        pickable.SetActive(false);
    }

}
