using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddSpinWeaponDamageReward : Rewardable
{
    public AddSpinWeaponDamageReward(Reward reward) : base(reward)
    {
    }
    public override void GetReward()
    {
        if (GameObject.FindObjectOfType<PlayerSpinWeapon>().IsHaveWeapon())
        {
            Debug.Log("增加玩家伤害");

            GameObject.FindObjectOfType<PlayerSpinWeapon>().AddSpinWeaponDamage(Reward.RewardValue);
        }
        else
        {
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
