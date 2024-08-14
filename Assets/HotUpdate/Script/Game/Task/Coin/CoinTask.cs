using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTask : Task
{
    public CoinTask(TaskData taskData) : base(taskData)
    {
    }
    public override void Reward()
    {
        if (info.state=="Compelet")
        {
           Debug.Log("奖励金币*2"+TaskData.RewardType);
        }
    }
}
