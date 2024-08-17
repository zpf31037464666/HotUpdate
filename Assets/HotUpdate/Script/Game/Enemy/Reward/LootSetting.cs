using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class LootSetting
{
    public GameObject prefab;
    [SerializeField, Range(0f, 100f)] public float dropPercentage;//生成率
    [SerializeField, Range(0f, 100f)] public int numebr;//生成个数
    public void Spaw(Vector3 postion)
    {
        for (int i = 0; i<numebr; i++)
        {
            if (Random.Range(0f, 100f)<=dropPercentage)
            {
                GameObject clone = ObjectPool.Instance.GetObject(prefab);
                clone.transform.position = postion;
            }
        }

    }
}

