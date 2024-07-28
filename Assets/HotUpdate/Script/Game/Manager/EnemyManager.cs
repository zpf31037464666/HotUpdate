using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float createTime;
    public float presistTime;

    [SerializeField] GameObject Enemy;
    [SerializeField] List<Transform> spawnList;

    private void Start()
    {
        StartCoroutine(CreateEnemyCorount());
    }

    IEnumerator CreateEnemyCorount()
    {
        var t = presistTime;
        while (t>0)
        {
            Transform spawn= spawnList[Random.Range(0, spawnList.Count)];

            GameObject enemy = ObjectPool.Instance.GetObject(Enemy);
            enemy.transform.position= spawn.position;


            yield return new WaitForSeconds(createTime);
        }
    }


}
