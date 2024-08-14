
using System;
using UnityEngine;

public interface ITask 
{
    string Name {  get; }
    TaskData TaskData { get; }

    void ReturnTaskInfo(Action<TaskInfo> callback);
    void UpdateState(int value);

    void Reward();
}
