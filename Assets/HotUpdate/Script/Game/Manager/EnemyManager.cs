using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float createTime;

    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject corssPrefab;
    [SerializeField] List<Transform> spawnList;

    WaveUI waveUI;

    List<Vector2> spawnPosList = new List<Vector2>();
    List<GameObject> enemyList = new List<GameObject>();

    float spwanRadius = 5;
    float waitSpawnWarningTime = 1f;
    private void Awake()
    {
        waveUI=FindAnyObjectByType<WaveUI>();
    }

    IEnumerator Start()
    {
        while (GameManager.GameState!= GameState.GameOver)
        {
            yield return StartCoroutine(CreateEnemyCorount());
        }
    }

    IEnumerator CreateEnemyCorount()
    {
        yield return new WaitUntil(() => GameManager.GameState== GameState.Playing);
        var t =(float) waveUI.rewardTime;
        while (t>0)
        {
            yield return StartCoroutine(SpawnEnemiesCoroutine(5));

            t-=createTime;
            yield return new WaitForSeconds(createTime);

            if(GameManager.GameState!= GameState.Playing)
            {
                yield break;
            }

        }
     
    }


    IEnumerator SpawnEnemiesCoroutine(int enemyNum)
    {
        spawnPosList.Clear();
        enemyList.Clear();
        for (int i = 0; i < enemyNum; i++)
        {
           // spawnPosList.Add(spawnList[Random.Range(0, spawnList.Count)].position + Random.insideUnitSphere * spwanRadius);
            spawnPosList.Add(spawnList[Random.Range(0, spawnList.Count)].position);
            enemyList.Add(Enemy);
        }


        for (int i = 0; i < enemyNum; i++)
        {
            yield return new WaitForSeconds(.2f);

            var clone=  ObjectPool.Instance.GetObject(corssPrefab);
            clone.transform.position=spawnPosList[i];
        }

        yield return new WaitForSeconds(waitSpawnWarningTime);

        for (int i = 0; i < enemyNum; i++)
        {
            var clone = ObjectPool.Instance.GetObject(enemyList[i]);
            clone.transform.position=spawnPosList[i];
        }
    }


}
