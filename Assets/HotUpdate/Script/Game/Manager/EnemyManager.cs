using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject corssPrefab;
    [SerializeField] List<Transform> spawnList;

    WaveUI waveUI;

    List<Vector2> spawnPosList = new List<Vector2>();

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
        var enemyWaveList = EnemyWaveManager.instance.GetEnemyWaveData(waveUI.waveNumber);
        float t = enemyWaveList[0].RewardTime;
        int currentIndex = 0;
        while (t > 0)
        {  
             yield return  StartCoroutine(SpawnEnemiesCoroutine(enemyWaveList[currentIndex].Count, enemyWaveList[currentIndex].EnemyType));
             currentIndex++;
             currentIndex %= enemyWaveList.Count;
            yield return new WaitForSeconds(enemyWaveList[currentIndex].SpawnTime);
            t-=enemyWaveList[currentIndex].SpawnTime;
            if (GameManager.GameState != GameState.Playing)
            {
                yield break;
            }
        }
    }
    IEnumerator SpawnEnemiesCoroutine(int enemyNum, string enemyPrefabPath)
    {
        spawnPosList.Clear();
        // 随机出生地
        for (int i = 0; i < enemyNum; i++)
        {
            spawnPosList.Add(spawnList[Random.Range(0, spawnList.Count)].position);
        }

        // 生成红叉
        for (int i = 0; i < enemyNum; i++)
        {
            yield return new WaitForSeconds(0.2f); // 等待时间

            var clone = ObjectPool.Instance.GetObject(corssPrefab); // 从对象池中获取红叉
            clone.transform.position = spawnPosList[i];
        }

        yield return new WaitForSeconds(waitSpawnWarningTime); // 等待警告时间

        // 生成僵尸
        for (int i = 0; i < enemyNum; i++)
        {
            if (GameManager.GameState != GameState.Playing)
            {
                yield break; // 如果游戏状态不是正在进行，则退出
            }

            // 从对象池中获取僵尸对象
            var clone = ObjectPool.Instance.GetObject(enemyPrefabPath); // 如果您使用的是 Addressables，请确保在对象池中有相应的处理

            // 检查获取的对象是否有效
            if (clone != null)
            {
                // 确保敌人对象在敌人列表中
                var enemyComponent = clone.GetComponent<Enemy>();
                clone.transform.position = spawnPosList[i]; // 设置敌人位置
            }
            else
            {
                Debug.LogError("Failed to get enemy from object pool.");
            }
        }
    }


}
