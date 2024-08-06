using System;
using UnityEngine;

public class AddMpReward : Rewardable
{
    public AddMpReward(Reward reward) : base(reward)
    {

    }
    public override void GetReward()
    {
        GameObject.FindAnyObjectByType<Player>().AddMp(Reward.RewardValue);

    }
}

