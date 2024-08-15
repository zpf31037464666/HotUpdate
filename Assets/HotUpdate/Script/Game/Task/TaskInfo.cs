
using System;
using UnityEngine;

public class TaskInfo
{
    public string name;       // 奖励名称
    public Sprite iconSprite; // 头像图片
    public string description; // 描述

    public int currentValue;
    public int targetValue;
    public string state;

    public string rewardName;
    public int rewardValue;
    public Sprite rewardSprite;

    public  Action rewardAction;
}
