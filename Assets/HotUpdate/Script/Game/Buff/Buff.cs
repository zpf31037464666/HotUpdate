using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : IBuff
{
    public string Name => GetType().Name;
    public BuffData buffData { get; set; }
    public Buff(BuffData buffData)
    {
        this.buffData = buffData;

        duationTimer=buffData.duration;
        tickTimer=buffData.tickTime;
        curStack=buffData.curStack;
    }

    public GameObject ower;
    public GameObject target;

    public float duationTimer;//持续时间
    public float tickTimer;//间隔时间
    public int curStack;//当前层数
    public void ReturnBuffDataInfo(Action<BuffData> callback)
    {

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
