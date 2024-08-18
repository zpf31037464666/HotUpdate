using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Task : ITask
{
    public string Name => GetType().Name;

    public TaskData taskData { get; set; }//数据层


    public TaskInfo info;

    public Task(TaskData taskData)
    {
       this.taskData=taskData;

        info=new TaskInfo();
        info.name=taskData.Name;
        info.description=taskData.Description;
        info.currentValue=taskData.CurrentCount;
        info.targetValue=taskData.TargetCount;
        info.state=taskData.State;

        info.rewardName=taskData.RewardName;
        info.rewardValue=taskData.RewardValue;
        info.rewardAction+=Reward;
    }

    public virtual void UpdateState(int value)
    {
        Debug.Log(info.name+"正在更新");
        info.currentValue+=value;

        taskData.CurrentCount=info.currentValue;

     
        Debug.Log(GetType().Name+"更新值为"+info.currentValue);
        if (info.currentValue >= info.targetValue)
        {
            info.state="完成";

            taskData.State="完成";

            Debug.Log(GetType().Name+"完成");
        }
    }
    public virtual void Reward()
    {
        if (info.state=="完成")
        {
            //临时
            TaskManager.instance.ReMoveTask(taskData.Id);

            Effect();
        }
    }
    public virtual void Effect()
    {
        
    }
    public void ReturnTaskInfo(Action<TaskInfo> callback)
    {
        Addressables.LoadAssetAsync<Sprite>(taskData.IconPath).Completed+=(handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                 info.iconSprite= handle.Result;

                Addressables.LoadAssetAsync<Sprite>(taskData.RewardIconPath).Completed+=(handle) =>
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
