using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Poison : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Buff buff = BuffManager.instance.GetBuff("毒药");

            if (buff != null)
            {
                Debug.Log("获得实例成功");

            }
            buff.ReturnBuffDataInfo((info) =>
            {
        
                Debug.Log("毒药的描述是"+info.description);

            });
            buff.Apply(collision.gameObject);
            collision.GetComponent<BuffHandle>().AddBuff(buff);

            Destroy(gameObject);
        }


    }





}
