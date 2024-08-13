using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddSpecialObjectReward : Rewardable
{
    public AddSpecialObjectReward(Reward reward) : base(reward)
    {
    }
    public override void GetReward()
    {
        AsyncOperationHandle<GameObject> bgHandle = Addressables.LoadAssetAsync<GameObject>(Reward.AddObjcetName);
        bgHandle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                //增加篮球
                GameObject weapon = GameObject.Instantiate(op.Result);
                Player player =  GameObject.FindAnyObjectByType<Player>();
                Bullet bullet= weapon.GetComponent<Bullet>();
                WeaponInfo weaponInfo=new WeaponInfo();
                weaponInfo.damage=10;
                weaponInfo.speed=20;

                bullet.GetComponent<Bullet>().SetPlayer(player);
                bullet.GetComponent<Bullet>().SetBulletInfo(weaponInfo);
               // bullet.GetComponent<Bullet>().SetDiction(new Vector2(Random.Range(0,1), Random.Range(0, 1)));
                bullet.GetComponent<Bullet>().SetDiction(Vector2.one*Random.Range(0.5f, 1));
            }
            else
            {
                Debug.LogError($"Failed to load sprite with key {Reward.BG}");
            }
        };
    }
}
