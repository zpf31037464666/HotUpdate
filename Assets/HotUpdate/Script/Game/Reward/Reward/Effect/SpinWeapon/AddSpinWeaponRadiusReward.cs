using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddSpinWeaponRadiusReward : Rewardable
{
    public AddSpinWeaponRadiusReward(Reward reward) : base(reward)
    {
    }
    public override void GetReward()
    {
        if (GameObject.FindObjectOfType<PlayerSpinWeapon>().IsHaveWeapon())
        {
            GameObject.FindObjectOfType<PlayerSpinWeapon>().AddSpinWeaponRadius(Reward.RewardValue);
        }
        else
        {
            //增加火球
            AsyncOperationHandle<GameObject> bgHandle = Addressables.LoadAssetAsync<GameObject>(Reward.AddObjcetName);
            bgHandle.Completed += (op) =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject weapon = op.Result;
                    GameObject.FindAnyObjectByType<PlayerSpinWeapon>().AddSpinWeapon(weapon);
                }
                else
                {
                    Debug.LogError($"Failed to load sprite with key {Reward.BG}");
                }
            };
            Debug.Log("增加旋转武器");
        }
           
    }
}
