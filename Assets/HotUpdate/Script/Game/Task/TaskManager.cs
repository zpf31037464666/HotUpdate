using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TaskManager : PersistentSingleton<TaskManager> ,ISaveable<List<TaskData>>
{
    public List<TaskData> taskDataList = new List<TaskData>();
    public List<Task> taskList = new List<Task>();//总任务列表

    public List<TaskData> owerDataList = new List<TaskData>();//拥有的任务的数据
    public List<Task> owerTaskList = new List<Task>();//拥有的任务
    private void Start()
    {
        RegisterSaveData();
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

             LoadSaveData();//加载本地数据
            //Debug.Log("owerTask 临时的处理");
            //owerTaskList=taskList;
            //UpateOverTaskData();

        }
        else
        {
            Debug.LogError("Failed to load JSON: " + handle);
        }
    }
    //实例化任务
    private void GrantTask(int id)
    {
        TaskData TaskData = taskDataList.Find(r => r.Id == id);
        Task task = TaskFactory.GetTask(TaskData.TaskType, TaskData);
        if(!taskList.Contains(task))
        {
            taskList.Add(task);
        }
    }
    private Task GetTask(int id)//获得初始任务
    {
        return taskList.Find(r=>r.taskData.Id == id);
    }

    /// <summary>
    /// 更新拥有的任务类型
    /// </summary>
    /// <param name="taskType"></param>
    /// <param name="value"></param>
    public void UpdateTaskState(string taskType, int value)
    {
        foreach (var task in owerTaskList)
        {
            if (task.taskData.TaskType == taskType)
            {
                Debug.Log("更新任务状态");
                task.UpdateState(value);
            }
        }
        UpateOverTaskData();//更新拥有任务的数据
        SaveData();
    }
    public void ReMoveTask(int id)
    {
        Task delectTask = new Task(new TaskData());
        foreach (var task in owerTaskList)
        {
            if (task.taskData.Id == id)
            {
                Debug.Log("移除奖励"+task.taskData.Name);
                delectTask = task;
            }
        }
        owerTaskList.Remove(delectTask);

        UpateOverTaskData();//更新拥有任务的数据
        SaveData();
    }
    /// <summary>
    /// 增加新任务
    /// </summary>
    /// <param name="id"></param>
    public void AddTask(int id)
    {
        foreach (var task in taskList)
        {
            if (task.taskData.Id == id)
            {
                Debug.Log("增加新任务");
                owerTaskList.Add(task);
            }
        }
        UpateOverTaskData();//更新拥有任务的数据
        SaveData();
    }
    private void UpateOverTaskData()
    {
        owerDataList.Clear();
        foreach (var task in owerTaskList)
        {
            owerDataList.Add(task.taskData);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("测试更新金币任务");
            UpdateTaskState("CoinTask", 1);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("测试增加任务任务");
            AddTask(0);
            AddTask(1);
        }
    }
    private void RegisterSaveData()
    {
        var playerSaveManager = SaveLoadManager<List<TaskData>>.GetInstance(GetType().Name);
        playerSaveManager.Register(this);
    }
    private void SaveData()
    {
        var playerSaveManager = SaveLoadManager<List<TaskData>>.GetInstance(GetType().Name);
        playerSaveManager.SaveGameData("Save1", GetType().Name);
    }
    private void LoadSaveData()
    {
        //加载场景
        var playerSaveManager = SaveLoadManager<List<TaskData>>.GetInstance(GetType().Name);
        playerSaveManager.LoadGameData("Save1", GetType().Name);
    }
    public string GetDataID()
    {
        return GetType().Name;
    }
    public List<TaskData> GenerateGameData()
    {
       return owerDataList;
    }

    public void RestoreGameData(List<TaskData> data)
    {
        owerDataList = data;
        
        owerTaskList.Clear();

        foreach (TaskData taskData in owerDataList)
        {
            Task task = TaskFactory.GetTask(taskData.TaskType, taskData);
            if (!owerTaskList.Contains(task))
            {
                owerTaskList.Add(task);
            }

        }
    }
}
