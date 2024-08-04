using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealthBuff : Buff
{
    public AddHealthBuff(BuffData buffData) : base(buffData)
    {
    }
    public override void OnEnter()
    {
        Debug.Log("进入时增加血量");
        ower.GetComponent<Player>()?.AddHealth(buffData.BuffValue);

    }
    public override void OnUpdate()
    {
        Debug.Log("更新血量");
        ower.GetComponent<Player>()?.AddHealth(buffData.BuffValue);
    }
    public override void OnRemove()
    {
        Debug.Log("移除时更新血量");
        ower.GetComponent<Player>()?.AddHealth(buffData.BuffValue);
    }
}

