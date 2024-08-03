using System;
using UnityEngine;
public interface IBuff 
{
    string Name { get; }
    BuffData buffData { get; set; }
    /// <summary>
    /// 异步返回奖励信息的方法
    /// </summary>
    /// <param name="callback">加载完成后的回调，返回加载的奖励信息</param>
    void ReturnBuffDataInfo(Action<BuffData> callback);
    void OnEnter();
    void OnUpdate();
    void OnRemove();
}
