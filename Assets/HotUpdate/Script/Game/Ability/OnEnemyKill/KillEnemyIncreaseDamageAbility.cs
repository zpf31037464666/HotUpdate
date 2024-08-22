using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemyIncreaseDamageAbility : KillEnemyAbillity
{
    public KillEnemyIncreaseDamageAbility(AbilityData data) : base(data)
    {
    }
    public override void Activate(Player player)
    {
        info.currentValue++;
        Debug.Log($"KillEnemyIncreaseDamageAbility 击杀数量: {info.currentValue}");
        if (info.currentValue >= info.targetValue)
        {
            player.AddAttackDamage(data.Value);//增加武器伤害
            info.currentValue = 0; // 重置击杀计数
        }
    }
}
