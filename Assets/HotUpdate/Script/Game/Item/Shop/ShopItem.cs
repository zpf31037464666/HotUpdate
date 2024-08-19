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

    private Button shopItemButton;
    private float doubleTime = .5f;
    bool isDouble = false;

    private void Awake()
    {
        shopItemButton = GetComponent<Button>();
    }
    IEnumerator DoubleCoroutine()
    {
        isDouble=true;
        yield return new WaitForSeconds(doubleTime);
        isDouble=false;
    }

    public void SetInfo(ShopItemInfo info)
    {
        nameText.text = info.name;
        IconImage.sprite=info.iconSprite;
        bg.sprite=info.bgSprite;
        priceImage.sprite=info.priceSprite;
        priceText.text=info.price.ToString();

        //buyButton.onClick.AddListener(() =>
        //{
        //    info.action?.Invoke();
        //});
    }
    public void AddShopItemDoubleButtonEvent(Action action)
    {
        shopItemButton.onClick.AddListener(() =>
        {
            if (isDouble)
            {
                action?.Invoke();
                return;
            }
            StartCoroutine(DoubleCoroutine());
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
