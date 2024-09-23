using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : UIState
{
    [SerializeField] Button closeButton;
    [SerializeField] private Task_UIAnimatorCotrol task_UIAnimatorCotrol;
    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ReturnToPreviousPanel();
        });
        TaskManager.instance.onTaskListChanged+=OnTaskListChanged;
    }
    private void OnDestroy()
    {
        TaskManager.instance.onTaskListChanged-=OnTaskListChanged;
    }
    public override void Enter()
    {
        base.Enter();

        task_UIAnimatorCotrol.UpdateUI(TaskManager.instance.owerTaskList);
    }

    private void OnTaskListChanged(List<Task> list)
    {
        Debug.LogWarning("TaskManager.instance.owerTaskList"+TaskManager.instance.owerTaskList.Count.ToString());

        if (TaskManager.instance.owerTaskList.Count<=0)
        {
            Debug.LogWarning("owerTaskList 列表为0");
            task_UIAnimatorCotrol.RefreshDataNumber(0);
            task_UIAnimatorCotrol.ClearData();
        }
        else
        {
            Debug.LogWarning("更新TaskPanel"+TaskManager.instance.owerTaskList.Count.ToString());
            task_UIAnimatorCotrol.UpdateUI(list);
        }
    }


}
