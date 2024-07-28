using System;
using UnityEngine;


public class AddLevelReward : Rewardable
{
    public AddLevelReward(Reward reward) : base(reward)
    {
    }
    public override void GetReward()
    {
        Debug.Log("增加经验");
    }
}

