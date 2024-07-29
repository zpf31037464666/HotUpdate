using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWeaponDamageReward : Rewardable
{
    public AddWeaponDamageReward(Reward reward) : base(reward)
    {
    }
    public override void GetReward()
    {
        GameObject.FindAnyObjectByType<PlayerWeapon>().AddWeaponDamage(Reward.RewardValue);
    }
}
