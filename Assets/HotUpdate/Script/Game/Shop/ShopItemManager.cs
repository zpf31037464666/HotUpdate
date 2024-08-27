using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ShopItemManager : PersistentSingleton<ShopItemManager>, ISaveable<List<ShopItemData>>
{
    public List<ShopItemData> dataList = new List<ShopItemData>();
    public List<ShopItemBase> shopItemList = new List<ShopItemBase>();

    private void Start()
    {
        LoadJson();
        RegisterSaveData();
        foreach (string name in ShopItemFactory.GetNames())
        {
            Debug.Log("ShopItemManager 具体  "+   name);
        }
    }
    private void LoadJson()
    {
        // 使用 Addressables 异步加载 JSON 文件
        Addressables.LoadAssetAsync<TextAsset>("Data/ShopItemData.json").Completed += OnJsonLoaded;
    }
    private void OnJsonLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // JSON 文件加载成功
            TextAsset jsonAsset = handle.Result;
            string json = jsonAsset.text;
            dataList = JsonConvert.DeserializeObject<List<ShopItemData>>(json);
            foreach (var data in dataList)
            {
                GrantShopItem(data.Id);
            }
            // 在这里处理 JSON 数据
            LoadSaveData();//加载本地数据
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + handle);
        }
    }
    private void GrantShopItem(int id)
    {
        ShopItemData data = dataList.Find(r => r.Id == id);
        var baseShopItem = ShopItemFactory.GetShopItemBase(data.Type, data);
        if (!shopItemList.Contains(baseShopItem))
        {
            shopItemList.Add(baseShopItem);
        }
    }
    public List<ShopItemBase> GetItemsByTag(string tag)
    {
        return shopItemList.Where(ShopItemBase => ShopItemBase.shopItemData.Tag == tag).ToList();
    }

    public void RemoveShopItem(int id)
    {
        ShopItemData data=new ShopItemData();
        ShopItemBase delectData=new ShopItemBase(data);
        foreach (var item in shopItemList)
        {
            if(item.shopItemData.Id == id)
            {
                delectData = item;
                break;
            }
        }
        shopItemList.Remove(delectData);
        dataList.RemoveAll(delectData=>delectData.Id == id);
        SaveData();
    }


    private void RegisterSaveData()
    {
        var playerSaveManager = SaveLoadManager<List<ShopItemData>>.GetInstance(GetType().Name);
        playerSaveManager.Register(this);
    }
    private void SaveData()
    {
        var playerSaveManager = SaveLoadManager<List<ShopItemData>>.GetInstance(GetType().Name);
        playerSaveManager.SaveGameData("Save1", GetType().Name);
    }
    private void LoadSaveData()
    {
        //加载场景
        var playerSaveManager = SaveLoadManager<List<ShopItemData>>.GetInstance(GetType().Name);
        playerSaveManager.LoadGameData("Save1", GetType().Name);
    }
    public string GetDataID()
    {
       return GetType().Name;
    }
    public List<ShopItemData> GenerateGameData()
    {
        return dataList;
    }
    public void RestoreGameData(List<ShopItemData> data)
    {
        dataList = data;
        shopItemList.Clear();

        foreach (var item in dataList)
        {
            GrantShopItem(item.Id);
        }

    }
}
