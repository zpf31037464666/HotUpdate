using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BuffManager : Singleton<BuffManager>
{
    private List<BuffData> buffdataList = new List<BuffData>();
    private List<Buff> buffs = new List<Buff>();
    private void Start()
    {
        LoadJson();
        foreach (string name in BuffFacotry.GetRewardNames())
        {
            Debug.Log("buff 具体"+   name);
        }
    }
    private void LoadJson()
    {
        // 使用 Addressables 异步加载 JSON 文件
        Addressables.LoadAssetAsync<TextAsset>("Data/BuffData.json").Completed += OnJsonLoaded;
    }
    private void OnJsonLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // JSON 文件加载成功
            TextAsset jsonAsset = handle.Result;
            string json = jsonAsset.text;
            buffdataList = JsonConvert.DeserializeObject<List<BuffData>>(json);
            foreach (var buffdata in buffdataList)
            {
                GrantBuff(buffdata.id);
            }
            // 在这里处理 JSON 数据
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + handle);
        }
    }
    public void GrantBuff(int buffID)
    {
        BuffData buffdata = buffdataList.Find(r => r.id == buffID);

        Buff buff = BuffFacotry.GetBuff(buffdata.BuffType, buffdata);

        Resiter(buff);
    }
    private void Resiter(Buff buff)
    {
        if(!buffs.Contains(buff))
        {
            buffs.Add(buff);
        }
    }
    public Buff GetBuff(int buffID)
    {
        foreach (var buff in buffs)
        {
            if (buff.buffData.id == buffID)
            {
                return buff; // 找到匹配的 Buff，返回
            }
        }
        Debug.LogWarning($"Buff with ID {buffID} not found.");
        return null; // 如果没有找到，返回 null
    }
    public Buff GetBuff(string  buffName)
    {
        foreach (var buff in buffs)
        {
            if (buff.buffData.buffname == buffName)
            {
                return buff; // 找到匹配的 Buff，返回
            }
        }
        Debug.LogWarning($"Buff with buffName {buffName} not found.");
        return null; // 如果没有找到，返回 null
    }
}
