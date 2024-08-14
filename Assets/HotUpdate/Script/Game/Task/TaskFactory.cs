
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Reflection;
using System.Linq;

public class TaskFactory 
{
    private static Dictionary<string, Type> taskDiction;
    private static bool isInit => taskDiction != null;
    private static void Init()
    {
        if (isInit) return;
        Debug.Log("初始化buff脚本");

        var taskTypes = Assembly.GetAssembly(typeof(ITask)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsInterface && myType.IsSubclassOf(typeof(Task)));
        taskDiction = new Dictionary<string, Type>();
        foreach (var taskType in taskTypes)
        {
            var tempEffect = Activator.CreateInstance(taskType, new TaskData()) as Task;
            Debug.Log(tempEffect.Name);
            taskDiction.Add(tempEffect.Name, taskType);
        }
    }
    public static Task GetTask(string taskType, TaskData data)
    {
        Init();
        if (taskDiction.ContainsKey(taskType))
        {
            Type type = taskDiction[taskType];
            try
            {
                var buffInstance = Activator.CreateInstance(type, data) as Task;
                return buffInstance;
            }
            catch (Exception ex)
            {
                Debug.LogError($"无法创建奖励实例 {taskType}: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"未找到奖励类型 {taskType}");
        }
        return null;
    }
    public static IEnumerable<string> GetTaskNames()
    {
        Init();
        return taskDiction.Keys;
    }
}
