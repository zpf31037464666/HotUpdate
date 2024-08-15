using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    [SerializeField]  Image Bg;
    [SerializeField]  Image iconImage;
    [SerializeField]  Text taskNameText;
    [SerializeField]  Image precessImage;
    [SerializeField]  Text  precessText;
    [SerializeField]  Text taskDescriptionText;


    [SerializeField]  Image rewardImage;
    [SerializeField]  Text rewardText;
    [SerializeField]  Button receiveButton;
    [SerializeField]  Text  stateText;
    public void SetInfo(TaskInfo taskInfo)
    {
        iconImage.sprite=taskInfo.iconSprite;
        taskNameText.text=taskInfo.name;
        precessImage.fillAmount=(float)taskInfo.currentValue/taskInfo.targetValue;
        precessText.text= $"{taskInfo.currentValue}/{taskInfo.targetValue}";
        taskDescriptionText.text=taskInfo.description;

        rewardImage.sprite=taskInfo.rewardSprite;
        rewardText.text=$"{taskInfo.rewardName}*{taskInfo.rewardValue}";
        stateText.text=taskInfo.state;

        receiveButton.onClick.AddListener(()=>{
            taskInfo.rewardAction?.Invoke();
        });
    }

    public void AddReceiveButtonEvent(Action action)
    {
        receiveButton.onClick.AddListener(()=> {
            action?.Invoke();
        });

    }


}
