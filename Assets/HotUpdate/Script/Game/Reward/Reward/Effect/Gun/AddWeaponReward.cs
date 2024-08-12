using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddWeaponReward : Rewardable
{
    public AddWeaponReward(Reward reward) : base(reward)
    {
    }
    public override void GetReward()
    {
        AsyncOperationHandle<GameObject> bgHandle = Addressables.LoadAssetAsync<GameObject>(Reward.AddObjcetName);
        bgHandle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject weapon=op.Result;
                GameObject.FindAnyObjectByType<PlayerWeapon>().AddGun(weapon);
            }
            else
            {
                Debug.LogError($"Failed to load sprite with key {Reward.BG}");
            }
        };
        Debug.Log("增加武器");
    }
}
