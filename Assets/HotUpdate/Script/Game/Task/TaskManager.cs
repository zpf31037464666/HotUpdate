using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TaskManager : PersistentSingleton<TaskManager>
{
    public List<TaskData> taskDataList = new List<TaskData>();
    public List<Task> taskList = new List<Task>();

    private void Start()
    {
        LoadJson();
        foreach (string name in TaskFactory.GetTaskNames())
        {
            Debug.Log("task 具体"+   name);
        }
    }
    private void LoadJson()
    {
        // 使用 Addressables 异步加载 JSON 文件
        Addressables.LoadAssetAsync<TextAsset>("Data/TaskData.json").Completed += OnJsonLoaded;
    }
    private void OnJsonLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // JSON 文件加载成功
            TextAsset jsonAsset = handle.Result;
            string json = jsonAsset.text;
            taskDataList = JsonConvert.DeserializeObject<List<TaskData>>(json);
            foreach (var buffdata in taskDataList)
            {
                GrantTask(buffdata.Id);
            }
            // 在这里处理 JSON 数据
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + handle);
        }
    }

    private void GrantTask(int id)
    {
        TaskData TaskData = taskDataList.Find(r => r.Id == id);
        Task task = TaskFactory.GetTask(TaskData.TaskType, TaskData);
        if(!taskList.Contains(task))
        {
            taskList.Add(task);
        }
    }
    public Task GetTask(string name)
    {
        foreach(var task in taskList)
        {
            if(task.TaskData.Name == name)
            {
                return task;
            }
        }
        Debug.LogWarning($"Task with taskName {name} not found.");
        return null;
    }
    public Task GetTask(int id)
    {
        foreach (var task in taskList)
        {
            if (task.TaskData.Id == id)
            {
                return task;
            }
        }
        Debug.LogWarning($"Task with taskName {name} not found.");
        return null;
    }
    public void UpdateTaskState(string taskType, int value)
    {
        foreach (var task in taskList)
        {
            if (task.TaskData.TaskType == taskType)
            {
                Debug.Log("更新任务状态");
                task.UpdateState(value);
            }
        }
    }

    //public void UpdateTaskState(string name, int value)
    //{
    //    foreach (var task in taskList)
    //    {
    //        if (task.TaskData.Name == name)
    //        {
    //            Debug.Log("更新任务状态");

    //            task.UpdateState(value);
    //        }
    //    }
    //}
    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        Debug.Log("测试任务");
    //        UpdateTaskState("CoinTask", 1);
    //       // UpdateTaskState("CoinTask", 1);

    //    }
    //}

}
