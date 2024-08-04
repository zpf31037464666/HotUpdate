using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandle : MonoBehaviour
{
    public List<Buff> buffList = new List<Buff>();
    private void Update()
    {
        BuffTick();
    }
    public virtual void BuffTick()
    {
        if (buffList.Count <= 0) return;

        List<Buff> deleteList = new List<Buff>();

        foreach (var buff in buffList)
        {
            // Update tick timer
            if (buff.tickTimer <= 0)
            {
                buff.OnUpdate();
                buff.tickTimer = buff.buffData.tickTime; // Reset tick timer
            }
            else
            {
                buff.tickTimer -= Time.deltaTime;
            }

            // Check duration
            if (buff.duationTimer <= 0)
            {
                deleteList.Add(buff);
            }
            else
            {
                buff.duationTimer -= Time.deltaTime;
            }
        }
        // Remove expired buffs
        foreach (var buff in deleteList)
        {
            RemoveBuff(buff);
        }
    }
    public virtual void AddBuff(Buff buff)
    {
        Buff findBuffInfo = FindBuff(buff.buffData.id);
        if (findBuffInfo != null)
        {
            if(buff.buffData.updateTimeType=="Add")
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
    }

    protected Buff FindBuff(int id)
    {
        foreach (Buff buffInfo in buffList)
        {
            if (buffInfo.buffData.id == id)
            {
                return buffInfo;
            }
        }
        Debug.LogWarning($"Buff with ID {id} not found.");
        return null;
    }

    public virtual void RemoveBuff(Buff buffInfo)
    {
        if (buffList.Contains(buffInfo))
        {
            if (buffInfo.buffData.removeStackUpdateType=="Clear")
            {
                buffInfo.OnRemove();
                buffList.Remove(buffInfo);
            }
            else if(buffInfo.buffData.removeStackUpdateType=="Reduce")
            {
                buffInfo.curStack--;
                buffInfo.duationTimer=buffInfo.buffData.duration;
                if(buffInfo.curStack <= 0 )
                {
                    buffInfo.OnRemove();
                    buffInfo.curStack = buffInfo.buffData.curStack;
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
