using UnityEngine;

public class AddWeaponCriticalEffectReward : Rewardable
{
    public AddWeaponCriticalEffectReward(Reward reward) : base(reward)
    {
    }
    public override void GetReward()
    {
        GameObject.FindAnyObjectByType<PlayerWeapon>().AddWeaponCriticalEffect(Reward.RewardValue/100.0f);
    }
}
