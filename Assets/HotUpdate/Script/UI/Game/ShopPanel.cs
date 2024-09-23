using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
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


    private int currentCoins = 100; // 当前金币数量
    private float animatorPersistTime=1f;
    private float moveDuration = 0.5f; // 移动持续时间

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
        EnterAnimator();
        UpdateShopItemBase(ShopItemManager.instance.GetItemsByTag("Character"));
        Init();
    }
    public override void Exit()
    {
        rectTransform.DOLocalMove(new Vector2(-Screen.width, 0), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            canvas.enabled = false;
        });
    }

    void EnterAnimator()
    {
        rectTransform.anchoredPosition = new Vector2(-Screen.width, 0); // 屏幕外的位置
        Vector2 targetPosition = Vector2.zero; // 屏幕中心（0，0）
        // 执行移动动画
        rectTransform.DOLocalMove(targetPosition, moveDuration).SetEase(Ease.Linear);
    }

    void Init()
    {
        genText.text=PlayerDataManager.instance.GetGem().ToString();

        StartCoroutine(AnimateCoinIncrease(currentCoins, PlayerDataManager.instance.GetCoin(), animatorPersistTime)); // 动画持续时间为2秒
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
                        Debug.Log("ShopPanel钱不够");
                        MessageManager.instance.SendMeesage("ShopPanel钱不够");
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


   IEnumerator AnimateCoinIncrease(int startValue, int endValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 计算当前的金币数量
            float t = elapsedTime / duration; // 计算时间进度
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t)); // 线性插值
            UpdateCoinText(currentValue); // 更新文本显示

            elapsedTime += Time.deltaTime; // 增加经过的时间
            yield return null; // 等待下一帧
        }

        // 确保最后的值是目标值
        UpdateCoinText(endValue);
        currentCoins = endValue; // 更新当前金币数量
    }

    private void UpdateCoinText(int value)
    {
        coinText.text = value.ToString(); // 更新金币文本
    }
}
