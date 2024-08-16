using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelManager : PersistentSingleton<LevelManager>, ISaveable<List<LevelData>>
{
    public List<LevelData> levelDataList = new List<LevelData>();
    private void Start()
    {
        RegisterSaveData();

        LoadJson();
    }

    

    private void LoadJson()
    {
        // 使用 Addressables 异步加载 JSON 文件
        Addressables.LoadAssetAsync<TextAsset>("Data/LevelData.json").Completed += OnJsonLoaded;
    }
    private void OnJsonLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // JSON 文件加载成功
            TextAsset jsonAsset = handle.Result;
            string json = jsonAsset.text;
            levelDataList = JsonConvert.DeserializeObject<List<LevelData>>(json);
            foreach (var level in levelDataList)
            {
               Debug.Log(level.Name);
            }
            // 在这里处理 JSON 数据
            LoadSaveData(); // 尝试加载保存的数据
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + handle);
        }
    }

    public void UnlockLevel(int levelId)
    {
        LevelData level = levelDataList.Find(l => l.Id == levelId);
        if (level != null)
        {
            Debug.Log(level.Name+"解锁场景");
            level.IsUnLock = true;
        }
        //保存场景
        SaveData();

    }
    private void RegisterSaveData()
    {
        var playerSaveManager = SaveLoadManager<List<LevelData>>.GetInstance(GetType().Name);
        playerSaveManager.Register(this);
    }
    private void SaveData()
    {
        var playerSaveManager = SaveLoadManager<List<LevelData>>.GetInstance(GetType().Name);
        playerSaveManager.SaveGameData("Save1", GetType().Name);
    }
    private void LoadSaveData()
    {
        //加载场景
        var playerSaveManager = SaveLoadManager<List<LevelData>>.GetInstance(GetType().Name);
        playerSaveManager.LoadGameData("Save1", GetType().Name);
    }
    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        Debug.Log("测试解锁场景");
    //        UnlockLevel(1);
    //    }
    //}

    public string GetDataID()
    {
        return GetType().Name;
    }

    public List<LevelData> GenerateGameData()
    {
        return levelDataList;
    }

    public void RestoreGameData(List<LevelData> data)
    {
        Debug.Log("加载LevelData");

        this.levelDataList = data;
    }


}
