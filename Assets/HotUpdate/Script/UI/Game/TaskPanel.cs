using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : UIState
{
    [SerializeField] TaskItem taskItem;
    [SerializeField] Transform taskListContainer;

    [SerializeField] Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ReturnToPreviousPanel();
        });
    }
    public override void Enter()
    {
        base.Enter();
       // UpdateUI(TaskManager.instance.taskList);
       UpdateUI(GameManager.instance.owerTaskList);
    }

    public override void LogicUpdata()
    {
      //  UpdateUI(TaskManager.instance.taskList);
    }

    public void UpdateUI(List<Task> tasks)
    {
        // 清空当前任务列表
        foreach (Transform child in taskListContainer)
        {
            Destroy(child.gameObject);
        }
        // 更新 UI
        foreach (var task in tasks)
        {
            task.ReturnTaskInfo((info) => { });//加载图片

            GameObject clone = Instantiate(taskItem.gameObject, taskListContainer);
            TaskItem taskItemUI = clone.GetComponent<TaskItem>();
            taskItemUI.SetInfo(task.info);
            taskItemUI.AddReceiveButtonEvent(() =>
            {
                if (task.info.state=="完成")
                {
                    Debug.Log("按钮 任务完成");
                    UpdateUI(GameManager.instance.owerTaskList);
                }
            });


        }

    }




}
