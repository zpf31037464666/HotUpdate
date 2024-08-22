using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet : Bullet
{
    public override void OnTriggerEnter2D(Collider2D other)
    {
        //base.OnTriggerEnter2D(other);
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            // Buff poisonBuff = BuffManager.instance.GetNewBuff("毒药子弹");
            // Destroy(gameObject);
            ObjectPool.Instance.PushObject(gameObject);
           Buff poisonBuff = BuffManager.instance.GetNewBuff("剧毒子弹");
           poisonBuff.Apply(gameObject, other.gameObject);//设置拥有者和目标
           if( other.gameObject.TryGetComponent(out BuffHandle buffHandle))
           {
                buffHandle.AddBuff(poisonBuff);
           }
        }
    }
}
