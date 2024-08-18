using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTask : Task
{
    public CoinTask(TaskData taskData) : base(taskData)
    {
    }
    public override void Effect()
    {
        //测试 增加金币
        PlayerDataManager.instance.AddCoin(1000);

    }
}
