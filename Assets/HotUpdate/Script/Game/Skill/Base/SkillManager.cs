using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SkillManager : Singleton<SkillManager>
{
    private List<SkillData> skillDataList = new List<SkillData>();
    private List<Skill> skillList = new List<Skill>();
    private void Start()
    {
        LoadJson();
        foreach (string name in SkillFacotry.GetSkillName())
        {
            Debug.Log("技能 具体"+   name);
        }
    }
    private void LoadJson()
    {
        // 使用 Addressables 异步加载 JSON 文件
        Addressables.LoadAssetAsync<TextAsset>("SkillData").Completed += OnJsonLoaded;
    }
    private void OnJsonLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // JSON 文件加载成功
            TextAsset jsonAsset = handle.Result;
            string json = jsonAsset.text;
            skillDataList = JsonConvert.DeserializeObject<List<SkillData>>(json);
            foreach (var buffdata in skillDataList)
            {
                GrantSkill(buffdata.Id);
            }
            // 在这里处理 JSON 数据
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + handle);
        }
    }
    public void GrantSkill(int SkillId)
    {
        SkillData skillData = skillDataList.Find(r => r.Id == SkillId);

        Skill skill = SkillFacotry.GetSkill(skillData.SkillType, skillData);

        Resiter(skill);
    }
    private void Resiter(Skill skill)
    {
        if (!skillList.Contains(skill))
        {
            skillList.Add(skill);
        }
    }
    public Skill GetSkill(int skillId)
    {
        foreach (var skill in skillList)
        {
            if (skill.SkillData.Id== skillId)
            {
                return skill; // 找到匹配的 Buff，返回
            }
        }
        Debug.LogWarning($"Buff with ID {skillId} not found.");
        return null; // 如果没有找到，返回 null
    }
    public Skill GetSkill(string skillName)
    {
        foreach (var skill in skillList)
        {
            if (skill.SkillData.Name == skillName)
            {
                return skill; // 找到匹配的 Buff，返回
            }
        }
        Debug.LogWarning($"Buff with buffName {skillName} not found.");
        return null; // 如果没有找到，返回 null
    }


}
