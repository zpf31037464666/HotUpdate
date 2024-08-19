using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : UIState
{
    [SerializeField] private Text coinText;
    [SerializeField] private Text genText;
    [SerializeField] private Button backButton;

    [SerializeField] private Button characterButton;
    [SerializeField] private Button weaponButton;
    [SerializeField] private Button propsButton;


    [Header("Item")]
    [SerializeField] private Transform shopItemGroup;
    [SerializeField] private ShopItem shopItem;

    private void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ReturnToPreviousPanel();
        });

        characterButton.onClick.AddListener(() =>
        {
            UpdateShopItemBase(ShopItemManager.instance.GetItemsByTag("Character"));
        });
        weaponButton.onClick.AddListener(() =>
        {
            UpdateShopItemBase(ShopItemManager.instance.GetItemsByTag("Weapon"));
        });
        propsButton.onClick.AddListener(() =>
        {
            UpdateShopItemBase(ShopItemManager.instance.GetItemsByTag("Tool"));
        });
    }
    public override void Enter()
    {
        base.Enter();

        UpdateShopItemBase(ShopItemManager.instance.GetItemsByTag("Character"));

        Init();
    }
    void Init()
    {
        coinText.text=PlayerDataManager.instance.GetCoin().ToString();
        genText.text=PlayerDataManager.instance.GetGem().ToString();
    }


    public void UpdateShopItemBase(List<ShopItemBase> shopItemList)
    {
        foreach(Transform child in shopItemGroup)
        {
            Destroy(child.gameObject);
        }

        foreach (var shopItemBase in shopItemList)
        {
            shopItemBase.ReturnShopItemDataInfo((info) => //当图片加载完毕时
            {
                var clone = Instantiate(shopItem.gameObject, shopItemGroup);
                var item = clone.GetComponent<ShopItem>();
                item.SetInfo(info);
                item.AddButtonEvent(() =>
                {
                    //临时只有判断金币 ，后面根据类型来判断
                    if(!PlayerDataManager.instance.ComPareCoin(info.price))
                    {
                        Debug.Log("钱不够");
                        Init();

                        // 创建一个数据字典并传递 info
                        Dictionary<string, object> data = new Dictionary<string, object>
                        {
                             { "itemInfo", info } // 将物品信息放入字典中
                        };
                        // 打开详情面板并传递数据
                        UIManager.Instance.OpenPanelWithData(My_UIConst.ItemDetailPanel, data);
                    }
                    else
                    {
                        Init();

                        // 创建一个数据字典并传递 info
                        Dictionary<string, object> data = new Dictionary<string, object>
                        {
                             { "itemInfo", info } // 将物品信息放入字典中
                        };
                        // 打开详情面板并传递数据
                        UIManager.Instance.OpenPanelWithData(My_UIConst.ItemDetailPanel, data);
                    }
                });
                //双击
                item.AddShopItemDoubleButtonEvent(() =>
                {
                    // 创建一个数据字典并传递 info
                    Dictionary<string, object> data = new Dictionary<string, object>
                        {
                             { "itemInfo", info } // 将物品信息放入字典中
                        };
                    // 打开详情面板并传递数据
                    UIManager.Instance.OpenPanelWithData(My_UIConst.ItemDetailPanel, data);
                });
            });
        }
    }



}
