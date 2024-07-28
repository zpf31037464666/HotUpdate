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
        Debug.Log("增加金币");
    }
}
