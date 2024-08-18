using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemInfo 
{
    public string name;       // 奖励名称
    public Sprite iconSprite; // 头像图片
    public Sprite bgSprite; // 头像图片
    public Sprite priceSprite; // 头像图片
    public string description; // 描述
    public int price;

    public Action action;//触发事件
}
