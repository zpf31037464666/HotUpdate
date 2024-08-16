using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class GameManager : PersistentSingleton<GameManager>
{
    public static Action onGameOver;
    //设置属性 其他类可以访问和设置
    public static GameState GameState { get => instance.gameState; set => instance.gameState = value; }
    [SerializeField] GameState gameState = GameState.UI;

    public PlayerItemData currentPlyaerItemData;

    public string currentSelectSenceName;

    //public List<Task> owerTaskList=new List<Task>();

    //private void Start()
    //{
    //    owerTaskList=TaskManager.instance.taskList;
    //}

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        Debug.Log("测试gameManger任务");
    //        UpdateTaskState("CoinTask", 1);
    //    }
    //}
    //public void ReMoveTask(int id)
    //{
    //    Task delectTask=new Task(new TaskData());
    //    foreach (var task in owerTaskList)
    //    {
    //        if (task.TaskData.Id == id)
    //        {
    //            Debug.Log("移除奖励"+task.TaskData.Name);
    //            delectTask = task;
    //        }
    //    }
    //     owerTaskList.Remove(delectTask);
    //}
    //public void UpdateTaskState(string taskType, int value)
    //{
    //    foreach (var task in owerTaskList)
    //    {
    //        if (task.TaskData.TaskType == taskType)
    //        {
    //            Debug.Log("更新任务状态");
    //            task.UpdateState(value);
    //        }
    //    }
    //}
}
public enum GameState
{
    Playing,
    Timeline,
    Paused,
    GameOver,
    Reward,
    UI
}