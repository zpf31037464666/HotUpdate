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

    private Dictionary<int, Task> taskDictionary = new Dictionary<int, Task>();
    private Dictionary<int, TaskData> taskDataDictionary = new Dictionary<int, TaskData>();

    public event Action<List<Task>> onTaskListChanged = delegate { };

    protected override void Awake()
    {
        base.Awake();

        RegisterSaveData();
        LoadJson();


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
            CreateTaskDictionaries();
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

    private void CreateTaskDictionaries()
    {
        foreach (var task in taskList)
        {
            taskDictionary[task.taskData.Id] = task;
        }

        foreach (var taskData in taskDataList)
        {
            taskDataDictionary[taskData.Id] = taskData;
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
                Debug.Log("更新任务状态"+task.taskData.TaskType);
                task.UpdateState(value);
            }
        }
        onTaskListChanged?.Invoke(owerTaskList);
        UpdateOwnedTaskData();//更新拥有任务的数据
        SaveData();
    }

    private Task GetOwerTaskById(int id)
    {
        return owerTaskList.Find(t => t.taskData.Id == id);
    }
    private Task GetTaskById(int id)
    {
        return taskList.Find(t => t.taskData.Id == id);
    }
    public void AddTask(int id)
    {
        if (owerTaskList.Exists(t => t.taskData.Id == id))
        {
            Debug.Log("任务已经存在");
            return;
        }

        if (taskDictionary.TryGetValue(id, out var task))
        {
            owerTaskList.Add(task);
            onTaskListChanged?.Invoke(owerTaskList);
            UpdateOwnedTaskData();
            SaveData();
            Debug.Log("增加任务: " + task.taskData.Name);
        }
        else
        {
            Debug.Log("任务不存在");
        }
    }

    public void RemoveTask(int id)
    {
        if (GetOwerTaskById(id) is var task && task != null)
        {
            Debug.Log("移除奖励: " + task.taskData.Name);
            owerTaskList.RemoveAll(t => t.taskData.Id == id);
            onTaskListChanged?.Invoke(owerTaskList);
            UpdateOwnedTaskData();
            SaveData();
        }
        else
        {
            Debug.Log("奖励不存在");
        }
    }
    private void UpdateOwnedTaskData()
    {
        owerDataList.Clear();
        foreach (var task in owerTaskList)
        {
            var taskData = task.taskData;
            taskData.CurrentCount = task.info.currentValue;
            taskData.State = task.info.state;
            owerDataList.Add(taskData);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("测试任务");
            for (int i = 0;i<100;i++)
            {
                AddTask(i);
            }
       
            //AddTask(1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("移除任务");
            for (int i = 0; i<100; i++)
            {
                RemoveTask(i);
            }
        }
    }
    #region SaveData
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
    #endregion
}
