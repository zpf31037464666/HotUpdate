using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Task_UIAnimatorCotrol : UIAnimatorControl<TaskInfo>
{
    private bool isUpdating = false;
    private HashSet<int> currentlyLoadingTaskIds = new HashSet<int>(); // 用于存储当前正在加载的任务 ID
    protected override void Awake()
    {
        base.Awake();
    }

    public void UpdateUI(List<Task> tasks)
    {
        if (isUpdating) return; // 如果当前正在更新，则直接返回
        isUpdating = true;

        List<TaskInfo> list = new List<TaskInfo>();
        // 更新 UI
        foreach (var task in tasks)
        {
            task.ReturnTaskInfo((info) => //当图片加载完毕时    Addressables.LoadAssetAsync<Sprite>
            {
                if (tasks.Contains(task))
                {
                    Debug.Log(info.name+"加载完成");
                    list.Add(task.info);
                    UpdateData(list);
                }
            });
        }
        isUpdating = false; // 更新完成后重置标志
    }
}
