using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ShopItemBase : IShopItem
{
    public string Name => GetType().Name;

    public ShopItemData shopItemData { get; set; }

    public ShopItemBase(ShopItemData shopItemData)
    {
        this.shopItemData = shopItemData;

        info=new ShopItemInfo();
        info.name=shopItemData.Name;
        info.description=shopItemData.Description;
        info.price=shopItemData.Price;

        info.action+=Apply;
    }

    ShopItemInfo info;

    public virtual void Apply()
    {


    }

    public void ReturnShopItemDataInfo(Action<ShopItemInfo> callback)
    {
        Addressables.LoadAssetAsync<Sprite>(shopItemData.IconPath).Completed+=(handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                info.iconSprite= handle.Result;

                Addressables.LoadAssetAsync<Sprite>(shopItemData.BGPath).Completed+=(handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        info.bgSprite= handle.Result;

                        Addressables.LoadAssetAsync<Sprite>(shopItemData.PriceSpritePath).Completed+=(handle) =>
                        {
                            if (handle.Status == AsyncOperationStatus.Succeeded)
                            {
                                info.priceSprite= handle.Result;

                                callback?.Invoke(info);
                            }
                        };
                    }
                };
            }
        };
    }
}
