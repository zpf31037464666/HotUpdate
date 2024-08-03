using Codice.Client.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandle : MonoBehaviour
{
    public List<Buff> buffList = new List<Buff>();

    private void Update()
    {
        BuffTick();

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("测试buff");
            BuffData buffData = new BuffData();
            buffData.tickTime=0.5f;
            buffData.duration=10f;

            AddHealthBuff healthBuff=new AddHealthBuff(buffData);

            healthBuff.Apply(gameObject);
            AddBuff(healthBuff);

        }
    }
    public virtual void BuffTick()
    {
        if (buffList.Count <=0) return;
        List<Buff> deleteList = new List<Buff>();
        foreach (var buff in buffList)
        {
            if (buff.tickTimer<0)
            {
                buff.OnUpdate();
                buff.tickTimer = buff.buffData.tickTime;
            }
            else
            {
                buff.tickTimer-=Time.deltaTime;
            }
            
            if (buff.duationTimer<=0)
            {
                deleteList.Add(buff);
            }
            else
            {
                buff.duationTimer-=Time.deltaTime;
            }
        }
        foreach (var buff in deleteList)
        {
            RemoveBuff(buff);
        }
    }
    public void AddBuff(Buff buff)
    {
        Buff findBuffInfo = FindBuff(buff.buffData.id);
        if (findBuffInfo != null)
        {
            findBuffInfo.OnEnter();
        }
        else
        {
            //初始化
            buffList.Add(buff);
            buff.OnEnter();
        }
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
        return null;
    }
    public virtual void RemoveBuff(Buff buffInfo)
    {
        buffInfo.OnRemove();
        buffList.Remove(buffInfo);
    }


   
}
