using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ShopItemFactory 
{
    private static Dictionary<string, Type> shopItemDiction;
    private static bool isInit => shopItemDiction != null;
    private static void Init()
    {
        if (isInit) return;
        Debug.Log("初始化buff脚本");
        var shopItemTypes = Assembly.GetAssembly(typeof(IShopItem)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsInterface && myType.IsSubclassOf(typeof(ShopItemBase)));
        shopItemDiction = new Dictionary<string, Type>();
        foreach (var shopItemType in shopItemTypes)
        {
            var tempEffect = Activator.CreateInstance(shopItemType, new ShopItemData()) as ShopItemBase;
            Debug.Log(tempEffect.Name);
            shopItemDiction.Add(tempEffect.Name, shopItemType);
        }
    }
    public static ShopItemBase GetShopItemBase(string shopItemType, ShopItemData data)
    {
        Init();
        if (shopItemDiction.ContainsKey(shopItemType))
        {
            Type type = shopItemDiction[shopItemType];
            try
            {
                var instance = Activator.CreateInstance(type, data) as ShopItemBase;
                return instance;
            }
            catch (Exception ex)
            {
                Debug.LogError($"无法创建奖励实例 {shopItemType}: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"未找到奖励类型 {shopItemType}");
        }
        return null;
    }
    public static IEnumerable<string> GetNames()
    {
        Init();
        return shopItemDiction.Keys;
    }
}
