using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ShopItemManager : PersistentSingleton<ShopItemManager>
{
    public List<ShopItemData> dataList = new List<ShopItemData>();
    public List<ShopItemBase> shopItemList = new List<ShopItemBase>();

    private void Start()
    {
        LoadJson();
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

}
