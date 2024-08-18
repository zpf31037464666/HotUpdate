using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLockCharacterShopItem : ShopItemBase
{
    public UnLockCharacterShopItem(ShopItemData shopItemData) : base(shopItemData)
    {
    }
    public override void Apply()
    {
        if(PlayerDataManager.instance.ComPareCoin(shopItemData.Price))
        {
            PlayerItemManager.instance.UnlockCharacter(shopItemData.UnLockName);
            PlayerDataManager.instance.RemoveCoin(shopItemData.Price);

            Debug.Log("购买成功 解锁人物"+shopItemData.Name);
        }

    }
}
