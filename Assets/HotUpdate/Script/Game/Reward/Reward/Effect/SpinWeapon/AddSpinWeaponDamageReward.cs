using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpinWeaponDamageReward : Rewardable
{
    public AddSpinWeaponDamageReward(Reward reward) : base(reward)
    {
    }
    public override void GetReward()
    {
        GameObject.FindObjectOfType<PlayerSpinWeapon>().AddSpinWeaponDamage(Reward.RewardValue);
    }
}
