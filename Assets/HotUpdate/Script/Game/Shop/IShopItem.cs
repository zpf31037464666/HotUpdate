
using System;
using UnityEngine;

public interface IShopItem
{
    string Name { get; }

    ShopItemData shopItemData { get; set; }

    void Apply();

    void ReturnShopItemDataInfo(Action<ShopItemInfo> callback);
}
