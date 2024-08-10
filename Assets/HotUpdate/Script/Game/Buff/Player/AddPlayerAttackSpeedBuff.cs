using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayerAttackSpeedBuff : Buff
{
    public AddPlayerAttackSpeedBuff(BuffData buffData) : base(buffData)
    {
    }
    public override void OnEnter()
    {
        Debug.Log("进入时攻击速度"+(100-buffData.BuffValue)/100f);
        ower.GetComponent<Player>()?.AddAttackSpeed((100-buffData.BuffValue)/100f);
    }
    public override void OnUpdate()
    {
    }
    public override void OnRemove()
    {
        Debug.Log("移除时攻击速度");
        ower.GetComponent<Player>()?.DecreateAttackSpeed((100-buffData.BuffValue)/100f);
    }
}
