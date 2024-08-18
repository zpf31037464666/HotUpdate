using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerItemManager : PersistentSingleton<PlayerItemManager>, ISaveable<List<PlayerItemData>>
{
    public List<PlayerItemData> PlayerItemDataList = new List<PlayerItemData>();
    private void Start()
    {
        var playerSaveManager = SaveLoadManager<List<PlayerItemData>>.GetInstance(GetType().Name);
        playerSaveManager.Register(this);
        LoadJson();
    }
    private void LoadJson()
    {
        // 使用 Addressables 异步加载 JSON 文件
        Addressables.LoadAssetAsync<TextAsset>("Data/PlayerItemData.json").Completed += OnJsonLoaded;
    }
    private void OnJsonLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // JSON 文件加载成功
            TextAsset jsonAsset = handle.Result;
            string json = jsonAsset.text;
            PlayerItemDataList = JsonConvert.DeserializeObject<List<PlayerItemData>>(json);
            foreach (var level in PlayerItemDataList)
            {
                Debug.Log(level.Name);
            }
            // 在这里处理 JSON 数据
            LoadData();//尝试加载数据
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + handle);
        }
    }
    public void SaveData()
    {
        var playerSaveManager = SaveLoadManager<List<PlayerItemData>>.GetInstance(GetType().Name);
        playerSaveManager.SaveGameData("Save1", GetType().Name);
    }
    public void LoadData()
    {
        //加载场景
        var playerSaveManager = SaveLoadManager<List<PlayerItemData>>.GetInstance(GetType().Name);
        playerSaveManager.LoadGameData("Save1", GetType().Name);
    }

    public void UnlockCharacter(int id)
    {
        var character = PlayerItemDataList.Find(l => l.Id == id);
        if (character != null)
        {
            Debug.Log(character.Name+"解锁人物");
            character.IsUnLock = true;
        }
        //保存场景
        SaveData();
    }

    public void UnlockCharacter(string name)
    {
        var character = PlayerItemDataList.Find(l => l.Name == name);
        if (character != null)
        {
            Debug.Log(character.Name+"解锁人物");
            character.IsUnLock = true;
        }
        //保存场景
        SaveData();
    }

    public string GetDataID()
    {
        return GetType().Name;
    }
    public List<PlayerItemData> GenerateGameData()
    {
        return PlayerItemDataList;
    }
    public void RestoreGameData(List<PlayerItemData> data)
    {
        PlayerItemDataList = data;
    }
}
