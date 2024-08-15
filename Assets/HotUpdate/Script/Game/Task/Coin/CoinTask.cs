using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTask : Task
{
    public CoinTask(TaskData taskData) : base(taskData)
    {
    }
    //public override void Reward()
    //{
    //    if (info.state=="Compelet")
    //    {
    //        //测试
    //        UIManager.Instance.SwitchPanel(My_UIConst.MainMenuPanel);

    //        Debug.Log("奖励金币*2"+TaskData.RewardType);
    //    }
    //}
    public override void Effect()
    {
        //测试
        Debug.Log("奖励金币*2"+TaskData.RewardType);
    }
}
