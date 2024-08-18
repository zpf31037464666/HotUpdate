using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private Image IconImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Image priceImage;
    [SerializeField] private Text priceText;


    public void SetInfo(ShopItemInfo info)
    {
        nameText.text = info.name;
        IconImage.sprite=info.iconSprite;
        bg.sprite=info.bgSprite;
        priceImage.sprite=info.priceSprite;
        priceText.text=info.price.ToString();

        buyButton.onClick.AddListener(() =>
        {
            info.action?.Invoke();
        });
    }
    public void AddButtonEvent(Action action)
    {
        buyButton.onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }

}
