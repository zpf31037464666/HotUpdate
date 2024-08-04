using Aliyun.OSS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHealthBuff : Buff
{
    public TakeHealthBuff(BuffData buffData) : base(buffData)
    {
    }
    public override void OnEnter()
    {
        Debug.Log("进入时扣除血量");
        ower.GetComponent<Player>().TakeDamage(buffData.BuffValue);

    }
    public override void OnUpdate()
    {
        Debug.Log("更新扣除血量");
        ower.GetComponent<Player>().TakeDamage(buffData.BuffValue);
    }
    public override void OnRemove()
    {
        Debug.Log("移除时扣除更新血量");
        ower.GetComponent<Player>().TakeDamage(buffData.BuffValue);
    }
}
