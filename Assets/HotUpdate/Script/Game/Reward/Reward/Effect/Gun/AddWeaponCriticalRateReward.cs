using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWeaponCriticalRateReward : Rewardable
{
    public AddWeaponCriticalRateReward(Reward reward) : base(reward)
    {

    }
    public override void GetReward()
    {
        GameObject.FindAnyObjectByType<PlayerWeapon>().AddWeaponCriticalRate(Reward.RewardValue/100.0f);
    }
}
