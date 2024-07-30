using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpinWeaponRadiusReward : Rewardable
{
    public AddSpinWeaponRadiusReward(Reward reward) : base(reward)
    {
    }
    public override void GetReward()
    {
        GameObject.FindObjectOfType<PlayerSpinWeapon>().AddSpinWeaponRadius(Reward.RewardValue);
    }
}
