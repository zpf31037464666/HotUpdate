using System.Collections.Generic;
using System;
using UnityEngine;
using System.Reflection;
using System.Linq;
public class AbilityFactory
{
    private static Dictionary<string, Type> abilityDiction;
    private static bool isInit => abilityDiction != null;
    private static void Init()
    {
        if (isInit) return;
        Debug.Log("初始化buff脚本");

        var abilityTypes = Assembly.GetAssembly(typeof(IAbility)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsInterface && myType.IsSubclassOf(typeof(Ability)));
        abilityDiction = new Dictionary<string, Type>();
        foreach (var type in abilityTypes)
        {
            var tempEffect = Activator.CreateInstance(type, new AbilityData()) as Ability;
            Debug.Log(tempEffect.Name);
            abilityDiction.Add(tempEffect.Name, type);
        }
    }
    public static Ability GetAblility(string abilityType, AbilityData data)
    {
        Init();
        if (abilityDiction.ContainsKey(abilityType))
        {
            Type type = abilityDiction[abilityType];
            try
            {
                var instance = Activator.CreateInstance(type, data) as Ability;
                return instance;
            }
            catch (Exception ex)
            {
                Debug.LogError($"无法创建奖励实例 {abilityType}: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"未找到奖励类型 {abilityType}");
        }
        return null;
    }
    public static IEnumerable<string> GetNames()
    {
        Init();
        return abilityDiction.Keys;
    }

}

