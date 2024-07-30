using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddSpinWeaponSpeedReward : Rewardable
{
    public AddSpinWeaponSpeedReward(Reward reward) : base(reward)
    {
    }
    public override void GetReward()
    {
        GameObject.FindObjectOfType<PlayerSpinWeapon>().AddSpinWeaponSpeed(Reward.RewardValue);
    }
}
