// 定义奖励接口
using UnityEngine;
using System;

public interface IRewardable
{
    string Name { get; }
    Reward Reward { get; set; }
    /// <summary>
    /// 异步返回奖励信息的方法
    /// </summary>
    /// <param name="callback">加载完成后的回调，返回加载的奖励信息</param>
    void ReturnRewardInfo(Action<RewardInfo> callback);

    /// <summary>
    /// 授予奖励的方法
    /// </summary>
    void GetReward();
}
public class RewardInfo
{
    public string name;       // 奖励名称
    public Sprite bgSprite;   // 背景图片
    public Sprite headSprite; // 头像图片
    public string description; // 描述
}
