using System;
using UnityEngine;


public class AddLevelReward : Rewardable
{
    public AddLevelReward(Reward reward) : base(reward)
    {
    }
    public override void GetReward()
    {
        //玩家升级
        GameObject.FindAnyObjectByType<Player>().Upgrade();
    }
}

