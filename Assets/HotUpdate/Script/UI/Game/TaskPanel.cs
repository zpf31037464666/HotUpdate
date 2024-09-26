using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : UIState
{
    [SerializeField] Button closeButton;
    [SerializeField] private Task_UIAnimatorCotrol task_UIAnimatorCotrol;

    [Header("Animator Set")]
    [SerializeField] private float moveDuration = 1;
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
        EnterAnimator();
    }
    public override void Exit()
    {
        ExitAimator();
    }
    void EnterAnimator()
    {
        rectTransform.anchoredPosition = new Vector2(-Screen.width, 0); // 屏幕外的位置
        Vector2 targetPosition = Vector2.zero; // 屏幕中心（0，0）
        // 执行移动动画
        rectTransform.DOLocalMove(targetPosition, moveDuration).SetEase(Ease.Linear);
    }
    private void ExitAimator()
    {
        rectTransform.DOLocalMove(new Vector2(-Screen.width, 0), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            canvas.enabled = false;
        });
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
