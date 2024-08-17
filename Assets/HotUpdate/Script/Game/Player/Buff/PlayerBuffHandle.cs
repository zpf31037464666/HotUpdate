using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffHandle : BuffHandle
{
    public static event Action<BuffHandle> OnBuffEntry;
    public static event Action<BuffHandle> OnBuffUpdate;
    public static event Action<BuffHandle> OnBuffExit;

    public override void AddBuff(Buff buff)
    {
        Debug.Log("增加buff"+buff.buffData.buffname);

        Buff findBuffInfo = FindBuff(buff.buffData.id);
        if (findBuffInfo != null)
        {
            if (buff.buffData.updateTimeType=="Add")
            {
                findBuffInfo.curStack+=buff.buffData.curStack;
                findBuffInfo.duationTimer=buff.buffData.duration;
            }
            if (buff.buffData.updateTimeType=="Replace")
            {
                findBuffInfo.duationTimer=buff.buffData.duration;
            }
            return;
        }
        buffList.Add(buff);
        buff.OnEnter();

        OnBuffEntry?.Invoke(this);

    }
    public override void BuffTick()
    {
        base.BuffTick();
        OnBuffUpdate?.Invoke(this);
    }
    public override void RemoveBuff(Buff buffInfo)
    {
        if (buffList.Contains(buffInfo))
        {
            if (buffInfo.buffData.removeStackUpdateType=="Clear")
            {
                OnBuffExit?.Invoke(this);//玩家自己的委托

                buffInfo.OnRemove();
                buffList.Remove(buffInfo);
            }
            else if (buffInfo.buffData.removeStackUpdateType=="Reduce")
            {
                buffInfo.curStack--;
                buffInfo.duationTimer=buffInfo.buffData.duration;
                if (buffInfo.curStack <= 0)
                {
                    OnBuffExit?.Invoke(this);//玩家自己的委托

                    //复原自己的层数
                    buffInfo.curStack = buffInfo.buffData.curStack;
                    buffInfo.OnRemove();
                    buffList.Remove(buffInfo);
                }
            }
        }
        else
        {
            Debug.LogWarning("Attempted to remove a buff that is not in the list.");
        }

    }

}
