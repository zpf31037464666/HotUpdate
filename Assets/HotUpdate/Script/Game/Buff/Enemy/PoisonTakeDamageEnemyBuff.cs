using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTakeDamageEnemyBuff : Buff
{
    public PoisonTakeDamageEnemyBuff(BuffData buffData) : base(buffData)
    {
    }
    Enemy enemy;
    public override void OnEnter()
    {
        Debug.Log("毒药攻击"+  target.gameObject.name);
        enemy = target?.GetComponent<Enemy>();

        if(enemy != null)
        {
            enemy.SetMaterial(Color.green);
            enemy.TakeDamage(buffData.BuffValue*curStack);
            DamageShowManager.instance.CreateDamage((int)buffData.BuffValue*curStack, enemy.gameObject.transform.position,Color.green);
        }
    }
    public override void OnUpdate()
    {
        Debug.Log("毒药攻击"+  target.gameObject.name);
        if (enemy != null)
        {
            enemy.SetMaterial(Color.green);
            enemy.TakeDamage(buffData.BuffValue*curStack);
            DamageShowManager.instance.CreateDamage((int)buffData.BuffValue*curStack, enemy.gameObject.transform.position, Color.green);
            Debug.Log("当前层数"+curStack);
            Debug.Log("给敌人造成"+buffData.BuffValue*curStack);
        }
    }
    public override void OnRemove()
    {
        Debug.Log("毒药攻击"+  target.gameObject.name);
        if (enemy != null)
        {
            enemy.RestoreMaterial();
            enemy.TakeDamage(buffData.BuffValue*curStack);
            DamageShowManager.instance.CreateDamage((int)buffData.BuffValue*curStack, enemy.gameObject.transform.position, Color.green);
        }
    }
}
