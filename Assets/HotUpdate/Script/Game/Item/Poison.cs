using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("测试buff");
        BuffData buffData = new BuffData();
        buffData.tickTime=0.5f;
        buffData.duration=10f;
        AddHealthBuff healthBuff = new AddHealthBuff(buffData);
        healthBuff.Apply(collision.gameObject);
        collision.GetComponent<BuffHandle>().AddBuff(healthBuff);

    }

}
