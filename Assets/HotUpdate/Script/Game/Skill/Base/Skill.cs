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

        //coolDownTime=skillData.CoolDownTime;
        coolDownTime=0;
    }
    public string Name => GetType().Name;

    public float coolDownTime;
    public SkillData SkillData { get ; set ; }
    public bool IsUse {
        get {
            return isUse= player? player.IsUseMp(info.mp):true; 
        }
        set { isUse=value; }
    }

    public SkillInfo info;

    Player player;

    private bool isUse;


    public virtual void Apply()
    {
        isUse= player.IsUseMp(info.mp);
        if (coolDownTime==0&&isUse)
        {
            coolDownTime = SkillData.CoolDownTime;
            player.UseMp(info.mp);
            Effect();
        }
    }
    public virtual void Effect()
    {
        if (SkillData.BuffName!="null")
        {
            Buff buff = BuffManager.instance.GetBuff(SkillData.BuffName);
            buff.ReturnBuffDataInfo((info) => {

                GameObject player = GameObject.FindAnyObjectByType<Player>().gameObject;
                buff.Apply(player);
                player.GetComponent<BuffHandle>().AddBuff(buff);

            });//加载图片完毕

        }
    }

    public void ReturnSkillDataInfo(Action<SkillInfo> callback)
    {
        player = GameObject.FindAnyObjectByType<Player>();
        info=new SkillInfo();
        info.name = SkillData.Name;
        info.description = SkillData.Description;
        info.mp=SkillData.UseMp;
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
