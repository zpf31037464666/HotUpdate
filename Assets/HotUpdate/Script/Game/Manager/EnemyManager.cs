using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float createTime;
    public int createNumber = 1;

    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject corssPrefab;
    [SerializeField] List<Transform> spawnList;

    WaveUI waveUI;

    List<Vector2> spawnPosList = new List<Vector2>();
    List<Enemy> enemyList = new List<Enemy>();

    float waitSpawnWarningTime = 1f;


    private void Awake()
    {
        waveUI = FindAnyObjectByType<WaveUI>();
    }

    private void OnEnable()
    {
        WaveUI.OnRewardEvent += onRewardEvent;
    }

    private void OnDisable()
    {
        WaveUI.OnRewardEvent -= onRewardEvent;
    }

    private void onRewardEvent()
    {
        Debug.Log("Rewad Event Clear Envet");

        foreach (var t in enemyList)
        {
            t.Die();
        }
       // enemyList.Clear();
    }

    IEnumerator Start()
    {
        while (GameManager.GameState != GameState.GameOver)
        {
            yield return StartCoroutine(CreateEnemyCorount());
        }
    }

    IEnumerator CreateEnemyCorount()
    {
        yield return new WaitUntil(() => GameManager.GameState == GameState.Playing);
        var t = (float)waveUI.rewardTime;
        while (t > 0)
        {
            // 保存协程的引用
              yield return  StartCoroutine(SpawnEnemiesCoroutine(createNumber));
            t -= createTime;
            yield return new WaitForSeconds(createTime);

            if (GameManager.GameState != GameState.Playing)
            {
                yield break;
            }
        }
    }

    IEnumerator SpawnEnemiesCoroutine(int enemyNum)
    {
        spawnPosList.Clear();
        for (int i = 0; i < enemyNum; i++)
        {
            // spawnPosList.Add(spawnList[Random.Range(0, spawnList.Count)].position + Random.insideUnitSphere * spwanRadius);
            spawnPosList.Add(spawnList[Random.Range(0, spawnList.Count)].position);
        }

        // 生成红叉
        for (int i = 0; i < enemyNum; i++)
        {
            yield return new WaitForSeconds(.2f);

            var clone = ObjectPool.Instance.GetObject(corssPrefab);
            clone.transform.position = spawnPosList[i];
        }
        yield return new WaitForSeconds(waitSpawnWarningTime);

        //生成僵尸
        for (int i = 0; i < enemyNum; i++)
        {
            if (GameManager.GameState != GameState.Playing)
            {
                yield break;
            }

            var clone = ObjectPool.Instance.GetObject(Enemy);
            if(!enemyList.Contains(clone.GetComponent<Enemy>()))
            {
                enemyList.Add(clone.GetComponent<Enemy>());
            }
            clone.transform.position = spawnPosList[i];
        }
    }
}
