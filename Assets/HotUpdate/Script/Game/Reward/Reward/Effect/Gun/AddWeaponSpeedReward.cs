using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWeaponSpeedReward : Rewardable
{
    public AddWeaponSpeedReward(Reward reward) : base(reward)
    {
    }
    public override void GetReward()
    {
        GameObject.FindAnyObjectByType<PlayerWeapon>().AddWeaponFireSpeed(Reward.RewardValue/100.0f);
    }
}
