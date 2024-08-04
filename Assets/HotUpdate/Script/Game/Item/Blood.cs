using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Buff buff = BuffManager.instance.GetBuff("血包");

            if (buff != null)
            {
                Debug.Log("获得实例成功");

            }
            buff.ReturnBuffDataInfo((info) =>
            {
             
  

            });
            buff.Apply(collision.gameObject);
            collision.GetComponent<BuffHandle>().AddBuff(buff);

            Destroy(gameObject);

        }


    }
}
