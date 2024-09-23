using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_UIAnimatorCotrol : UIAnimatorControl<TaskInfo>
{
    private bool isUpdating = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public void UpdateUI(List<Task> tasks)
    {
        if (isUpdating) return; // 如果当前正在更新，则直接返回

        // 统一标志
        isUpdating = true;

        // 启动协程处理异步数据加载
        StartCoroutine(LoadTasksInfo(tasks));
    }

    private IEnumerator LoadTasksInfo(List<Task> tasks)
    {
        Debug.LogWarning(tasks);
        List<TaskInfo> list = new List<TaskInfo>();
        foreach (var task in tasks)
        {
            task.ReturnTaskInfo((info) =>
            {
                if (tasks.Contains(task))
                {
                    Debug.LogWarning(info.name + "加载完成");
                    list.Add(info);
                }
            });
        }
        yield return null; // 等待下一帧

        // 一次性更新 UI
        UpdateData(list);
        isUpdating = false; // 更新完成后重置标志
    }
}
