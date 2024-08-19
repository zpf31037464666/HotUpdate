using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailPanel : UIState
{
    [SerializeField] private Image bg;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Image IconImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Image priceImage;
    [SerializeField] private Text priceText;
    [SerializeField] private Text descriptText;

    private void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ReturnToPreviousPanel();
        });
    }

    public void SetInfo(ShopItemInfo info)
    {
        nameText.text = info.name;
        IconImage.sprite=info.iconSprite;
        bg.sprite=info.bgSprite;
        priceImage.sprite=info.priceSprite;
        priceText.text=info.price.ToString();
        descriptText.text=info.description;
        buyButton.onClick.AddListener(() =>
        {
            info.action?.Invoke();
        });
    }
    public override void SetData(Dictionary<string, object> data)
    {
        if (data.ContainsKey("itemInfo") && data["itemInfo"] is ShopItemInfo itemInfo)
        {
            SetInfo(itemInfo);
        }
    }
}
