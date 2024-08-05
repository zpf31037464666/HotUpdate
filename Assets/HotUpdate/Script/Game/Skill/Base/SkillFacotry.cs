using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class SkillFacotry 
{
    private static Dictionary<string, Type> skillDiction;
    private static bool isInit => skillDiction != null;
    private static void Init()
    {
        if (isInit) return;
        Debug.Log("初始化buff脚本");

        var skillTypes = Assembly.GetAssembly(typeof(ISkill)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsInterface && myType.IsSubclassOf(typeof(Skill)));
        skillDiction = new Dictionary<string, Type>();
        foreach (var skilltype in skillTypes)
        {
            var tempEffect = Activator.CreateInstance(skilltype, new SkillData()) as Skill;
            Debug.Log(tempEffect.Name);
            skillDiction.Add(tempEffect.Name, skilltype);
        }
    }
    public static Skill GetSkill(string skillType, SkillData skillData)
    {
        Init();
        if (skillDiction.ContainsKey(skillType))
        {
            Type type = skillDiction[skillType];
            try
            {
                var buffInstance = Activator.CreateInstance(type, skillData) as Skill;
                return buffInstance;
            }
            catch (Exception ex)
            {
                Debug.LogError($"无法创建奖励实例 {skillType}: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"未找到奖励类型 {skillType}");
        }
        return null;
    }
    public static IEnumerable<string> GetSkillName()
    {
        //Debug.Log("获取所有技能名称");
        Init();
        return skillDiction.Keys;
    }
}
