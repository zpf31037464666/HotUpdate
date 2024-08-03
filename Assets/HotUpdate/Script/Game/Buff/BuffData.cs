using System;
using UnityEngine;
public class BuffData
{
    //基本信息
    public int id;
    public string buffname;
    public string description;
    public string icon;

    public int curStack;
    public int priority;
    public int maxStack;
    public string[] tags;
    //时间信息
    public bool isForever;
    public float duration;
    public float tickTime;
    //更新方式
    public string updateTimeType;
    public string removeStackUpdateType;
}
