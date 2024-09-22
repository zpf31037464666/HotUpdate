using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoinReward : Rewardable
{
    public AddCoinReward(Reward reward) : base(reward)
    {
    }
    public override void GetReward()
    {
       // MessageManager.instance.SendMeesage("增加金币"+Reward.RewardValue.ToString());
        PlayerDataManager.instance.AddCoin(Reward.RewardValue);
    }
}
