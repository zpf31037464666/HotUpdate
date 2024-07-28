using System;
using UnityEngine;

public class AddMpReward : Rewardable
{
    public AddMpReward(Reward reward) : base(reward)
    {

    }
    public override void GetReward()
    {
        Debug.Log("增加蓝量");

    }
}

