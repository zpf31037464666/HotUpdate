using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Ability:IAbility
{
    public string Name => GetType().Name;

    public AbilityData data { get; set; }//数据层

    public AbilityInfo info;

    public Ability(AbilityData data)
    {
        this.data=data;

        info=new AbilityInfo();
        info.name=data.Name;
        info.description=data.Description;
        info.currentValue=data.CurrentCount;
        info.targetValue=data.TargetCount;
    }

    public virtual void Activate(Player player)
    {
       
    }

    public void ReturnTaskInfo(Action<AbilityInfo> callback)
    {
        Addressables.LoadAssetAsync<Sprite>(data.IconPath).Completed+=(handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                info.iconSprite= handle.Result;
                callback?.Invoke(info);
            }
        };
    }
}

