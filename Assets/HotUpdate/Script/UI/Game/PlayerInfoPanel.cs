using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class PlayerInfoPanel : UIState
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button backButton;

    [SerializeField] private Image playerHeadImage;
    [SerializeField] private Text nameText;

    [Header("Attribute")]
    [SerializeField] private Text healthText;
    [SerializeField] private Text ArmorText;//护甲
    [SerializeField] private Text MPText;
    [SerializeField] private Text SpeedText;
    [SerializeField] private Text CriticalText;//暴击
    [SerializeField] private Text DodgeText;
    [SerializeField] private Text VampireText;//吸血率
    [SerializeField] private Text AttackSpeedText;

    [SerializeField] private Text SpecialText;

    [Header("Animator set")]
    private float moveDuration=0.5f;


    private void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.LevelSelectPanel);
        });

        backButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.SelectPlayerPanel);
        });
    }

    public override void Enter()
    {
        base.Enter();

        UpdateInfo(GameManager.instance.currentPlyaerItemData);
        EnterAnimator();
    }
    public override void Exit()
    {
        ExitAimator();
    }

    private void ExitAimator()
    {
        rectTransform.DOLocalMove(new Vector2(0, -Screen.height), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
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

    void UpdateInfo(PlayerItemData info)
    {
        nameText.text = info.Name;
        healthText.text=info.Health.ToString();
        ArmorText.text = info.Armor.ToString();
        MPText.text = info.MP.ToString();
        SpeedText.text= info.Speed.ToString();
        CriticalText.text = info.Critical.ToString();
        DodgeText.text = info.Dodge.ToString();
        VampireText.text = info.Vampire.ToString();
        AttackSpeedText.text = info.AttackSpeed.ToString();
        SpecialText.text = info.Special.ToString();

        Addressables.LoadAssetAsync<Sprite>(info.IconPath).Completed+=(handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                playerHeadImage.sprite= handle.Result;
            }
        };
    }


}
