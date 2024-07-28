using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class Rewardable : IRewardable
{
    public Reward Reward { get; set; }

    public string Name => GetType().Name;

    public Rewardable(Reward reward)
    {
        Reward = reward;
    }

    public void ReturnRewardInfo(Action<RewardInfo> callback)
    {
        RewardInfo info = new RewardInfo();
        info.name = Reward.RewardName;
        info.description = Reward.Descript;

        // 异步加载背景图
        AsyncOperationHandle<Sprite> bgHandle = Addressables.LoadAssetAsync<Sprite>(Reward.BG);
        bgHandle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                info.bgSprite = op.Result;
            }
            else
            {
                Debug.LogError($"Failed to load sprite with key {Reward.BG}");
            }

            // 异步加载图标
            AsyncOperationHandle<Sprite> iconHandle = Addressables.LoadAssetAsync<Sprite>(Reward.Icon);
            iconHandle.Completed += (opIcon) =>
            {
                if (opIcon.Status == AsyncOperationStatus.Succeeded)
                {
                    info.headSprite = opIcon.Result;
                }
                else
                {
                    Debug.LogError($"Failed to load sprite with key {Reward.Icon}");
                }

                // 调用回调，返回加载完成的 RewardInfo
                callback?.Invoke(info);
            };
        };
    }

    public virtual void GetReward()
    {
        // 实现领取奖励的逻辑
    }
}
