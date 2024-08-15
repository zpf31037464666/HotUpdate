using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Task : ITask
{
    public string Name => GetType().Name;

    public TaskData TaskData { get; set; }//数据层;

    public TaskInfo info;

    public Task(TaskData taskData)
    {
        TaskData=taskData;

        info=new TaskInfo();
        info.name=TaskData.Name;
        info.description=TaskData.Description;
        info.currentValue=TaskData.CurrentCount;
        info.targetValue=TaskData.TargetCount;
        info.state=TaskData.State;

        info.rewardName=TaskData.RewardName;
        info.rewardValue=TaskData.RewardValue;
        info.rewardAction+=Reward;
    }

    public virtual void UpdateState(int value)
    {
        Debug.Log(info.name+"正在更新");
        info.currentValue+=value;
        Debug.Log(GetType().Name+"更新值为"+info.currentValue);
        if (info.currentValue >= info.targetValue)
        {
            info.state="完成";

            Debug.Log(GetType().Name+"完成");
        }
    }
    public virtual void Reward()
    {
        if (info.state=="完成")
        {
            //临时
            GameManager.instance.ReMoveTask(TaskData.Id);
            Effect();
        }
    }
    public virtual void Effect()
    {
        
    }
    public void ReturnTaskInfo(Action<TaskInfo> callback)
    {
        Addressables.LoadAssetAsync<Sprite>(TaskData.IconPath).Completed+=(handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                 info.iconSprite= handle.Result;

                Addressables.LoadAssetAsync<Sprite>(TaskData.RewardIconPath).Completed+=(handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        info.rewardSprite= handle.Result;

                        callback?.Invoke(info);
                    }
                };
            }
        };
    }


}
