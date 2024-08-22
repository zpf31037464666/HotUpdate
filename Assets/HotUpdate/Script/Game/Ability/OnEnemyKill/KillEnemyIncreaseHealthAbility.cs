using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemyIncreaseHealthAbility : KillEnemyAbillity
{
    public KillEnemyIncreaseHealthAbility(AbilityData data) : base(data)
    {
    }
    public override void Activate(Player player)
    {
        info.currentValue++;
        Debug.Log($"KillEnemyIncreaseDamageAbility 击杀数量: {info.currentValue}");
        if (info.currentValue >= info.targetValue)
        {
            player.AddHelath(data.Value);//增加血量伤害
            info.currentValue = 0; // 重置击杀计数
        }
    }
}
