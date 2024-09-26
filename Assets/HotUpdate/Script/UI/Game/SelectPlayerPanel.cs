using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerPanel : UIState
{
    [SerializeField] private Button backButton;

    [SerializeField] private PlayerItem playerItem;
    [SerializeField] private Transform playerItemGroup;
    [Header("Show Stage")]
    [SerializeField] private Image showBgImage;
    [SerializeField] private Image headImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptText;
    [SerializeField] private Button startButton;

    [Header("Animator Set")]
    private float moveDuration = 0.5f; // 移动持续时间

    private  List<PlayerItemData> playerItemList=null;
    private void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.PlayerInfoPanel);
        });
        backButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.ComprehensivePanel);
        });
    }
    public override void Enter()
    {
        base.Enter();
        EnterAnimator();
        Init();
    }
    public override void Exit()
    {
        ExitAimator();
    }

    private void ExitAimator()
    {
        rectTransform.DOLocalMove(new Vector2(0,-Screen.height), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            canvas.enabled = false;
        });
    }

    void EnterAnimator()
    {
        rectTransform.anchoredPosition = new Vector2(0, Screen.height); // 屏幕外的位置
        Vector2 targetPosition = Vector2.zero; // 屏幕中心（0，0）
        // 执行移动动画
        rectTransform.DOLocalMove(targetPosition, moveDuration).SetEase(Ease.Linear);
    }


    void ClearChildPlayerItem()
    {
        foreach (Transform t in playerItemGroup)
        {
            Destroy(t.gameObject);
        }
    }

    void Init()
    {
        ClearChildPlayerItem();

        playerItemList =PlayerItemManager.instance.PlayerItemDataList;
        foreach (PlayerItemData item in playerItemList)
        {
            var clone=Instantiate(playerItem.gameObject, playerItemGroup);
            clone.GetComponent<PlayerItem>().Init(item, this);
        }

        PlayerItem slectPlayerItem= playerItemGroup.GetChild(0).GetComponent<PlayerItem>();
        slectPlayerItem.SelcetShowStaet();

    }

    public void SetShowStage(bool isUnlock, Sprite bgSprite, Sprite headSprite, string name, string descript,PlayerItemData playerItemData)
    {
            showBgImage.sprite = bgSprite;
            headImage.sprite = headSprite;
            nameText.text = name;
            descriptText.text = descript;
            lockImage.gameObject.SetActive(!isUnlock);
            startButton.interactable=isUnlock;

            //当前选择的人
            GameManager.instance.currentPlyaerItemData = playerItemData;
    }

}
