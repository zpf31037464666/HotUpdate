using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Buff : IBuff
{
    public string Name => GetType().Name;
    public BuffData buffData { get; set; }//数据层
    public Buff(BuffData buffData)
    {
        Debug.Log("初始化数据"+Name);
        this.buffData = buffData;

        duationTimer=buffData.duration;
        tickTimer=buffData.tickTime;
        curStack=buffData.curStack;
    }

    public GameObject ower;
    public GameObject target;

    public BuffInfo info;

    //其他类引用显示
    public float duationTimer;//持续时间
    public float tickTimer;//间隔时间
    public int curStack;//当前层数
    public void ReturnBuffDataInfo(Action<BuffInfo> callback)
    {    
        info=new BuffInfo();
        info.name = buffData.buffname;
        info.description = buffData.description;
        // 异步加载背景图
        AsyncOperationHandle<Sprite> bgHandle = Addressables.LoadAssetAsync<Sprite>(buffData.icon);
        bgHandle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                info.showSprite = op.Result;
                callback?.Invoke(info);
                Debug.Log("加载图片成功");
            }
            else
            {
                Debug.LogError($"Failed to load sprite with key {buffData.icon}");
            }
            // 异步加载图标
        };
    }
    public virtual void OnEnter()
    {
       
    }
    public virtual void OnRemove()
    {
       
    }
    public virtual void OnUpdate()
    {
       
    }
    public virtual void Apply(GameObject ower, GameObject target=null)
    {
        this.ower = ower;
        this.target = target;
    }
}
