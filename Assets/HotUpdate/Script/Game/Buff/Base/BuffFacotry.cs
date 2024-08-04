using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class BuffFacotry 
{
    private static Dictionary<string, Type> buffDiction;
    private static bool isInit => buffDiction != null;
    private static void Init()
    {
        if (isInit) return;
        Debug.Log("初始化buff脚本");

        var buffTypes = Assembly.GetAssembly(typeof(IBuff)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsInterface && myType.IsSubclassOf(typeof(Buff)));
        buffDiction = new Dictionary<string, Type>();
        foreach (var bufftype in buffTypes)
        {
            var tempEffect = Activator.CreateInstance(bufftype, new BuffData()) as Buff;
            Debug.Log(tempEffect.Name);
            buffDiction.Add(tempEffect.Name, bufftype);
        }
    }
    public static Buff GetBuff(string buffType, BuffData buffData)
    {
        Init();
        if (buffDiction.ContainsKey(buffType))
        {
            Type type = buffDiction[buffType];
            try
            {
                var buffInstance = Activator.CreateInstance(type, buffData) as Buff;
                return buffInstance;
            }
            catch (Exception ex)
            {
                Debug.LogError($"无法创建奖励实例 {buffType}: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"未找到奖励类型 {buffType}");
        }
        return null;
    }
    public static IEnumerable<string> GetRewardNames()
    {
        //Debug.Log("获取所有奖励名称");
        Init();
        return buffDiction.Keys;
    }
}
