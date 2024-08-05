using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Skill : ISkill
{
    public Skill(SkillData skillData)
    {
        this.SkillData = skillData;

        coolDownTime=skillData.CoolDownTime;
    }
    public string Name => GetType().Name;

    public float coolDownTime;
    public SkillData SkillData { get ; set ; }

    public SkillInfo info;
    public virtual void Apply()
    {

    }

    public void ReturnSkillDataInfo(Action<SkillInfo> callback)
    {
        info=new SkillInfo();
        info.name = SkillData.Name;
        info.description = SkillData.Description;
        // 异步加载背景图
        AsyncOperationHandle<Sprite> bgHandle = Addressables.LoadAssetAsync<Sprite>(SkillData.IconPath);
        bgHandle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                info.showSprite = op.Result;
                callback?.Invoke(info);
                Debug.Log("加载图片成功");
            }
            else
            {
                Debug.LogError($"Failed to load sprite with key {SkillData.IconPath}");
            }
            // 异步加载图标
        };
    }
}
