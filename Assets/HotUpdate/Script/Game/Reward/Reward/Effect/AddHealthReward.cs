using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealthReward : Rewardable
{
    public AddHealthReward(Reward reward) : base(reward)
    {
      
    }
    public override void GetReward()
    {
        GameObject.FindAnyObjectByType<Player>().AddHelath(Reward.RewardValue);

        Debug.Log("增加血量"+Reward.RewardValue);
    }
}
