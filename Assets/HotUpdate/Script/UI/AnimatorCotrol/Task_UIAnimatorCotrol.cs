using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_UIAnimatorCotrol : UIAnimatorControl<TaskInfo>
{
    private bool isUpdating = false;

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

        // 创建一个 List 用于存储所有的协程
        List<Coroutine> loadingCoroutines = new List<Coroutine>();

        foreach (var task in tasks)
        {
            // 启动一个新的协程来加载每个任务的信息
            Coroutine coroutine = StartCoroutine(LoadTaskInfo(task, list));
            loadingCoroutines.Add(coroutine);
        }

        // 等待所有的协程完成
        foreach (var coroutine in loadingCoroutines)
        {
            yield return coroutine;
        }

        // 一次性更新 UI
        UpdateData(list);
        isUpdating = false; // 更新完成后重置标志
    }

    private IEnumerator LoadTaskInfo(Task task, List<TaskInfo> list)
    {
        // 等待任务加载信息
        TaskInfo info = null;

        // 使用一个简单的标志来等待信息加载
        bool isLoaded = false;

        task.ReturnTaskInfo((taskInfo) =>
        {
            info = taskInfo;
            isLoaded = true;
        });

        // 等待直到信息加载完成
        while (!isLoaded)
        {
            yield return null; // 等待下一帧
        }

        if (info != null)
        {
            Debug.LogWarning(info.name + "加载完成");
            list.Add(info);
        }
    }
}