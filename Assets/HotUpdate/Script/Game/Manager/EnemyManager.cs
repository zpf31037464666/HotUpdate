using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float createTime;

    [SerializeField] GameObject Enemy;
    [SerializeField] List<Transform> spawnList;

    WaveUI waveUI;

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
            Transform spawn= spawnList[Random.Range(0, spawnList.Count)];
            GameObject enemy = ObjectPool.Instance.GetObject(Enemy);
            enemy.transform.position= spawn.position;
            t-=createTime;
            yield return new WaitForSeconds(createTime);

            if(GameManager.GameState!= GameState.Playing)
            {
                yield break;
            }

        }
     
    }


}
