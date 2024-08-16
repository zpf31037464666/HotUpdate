
using System;
using UnityEngine;

public interface ITask 
{
    string Name {  get; }
    TaskData taskData { get; set; }

    void ReturnTaskInfo(Action<TaskInfo> callback);
    void UpdateState(int value);

    void Reward();
}
