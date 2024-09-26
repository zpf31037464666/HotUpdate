using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ComprehensivePanel : UIState
{
    [SerializeField] Button gamePlayButton;
    [SerializeField] Button taskPaneButton;
    [SerializeField] Button shopPaneButton;


    [SerializeField] Text coinText;
    [SerializeField] Text gemText;

    private int currentCoins = 100; // 当前金币数量
    private float animatorPersistTime = 0.5f;
    private void Start()
    {
        gamePlayButton.onClick.AddListener(() =>
        {
            canvas.enabled = false;
            UIManager.Instance.SwitchPanel(My_UIConst.SelectPlayerPanel);
        });
        taskPaneButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.TaskPanel);
        });
        shopPaneButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.ShopPanel);
        });



    }
    public override void Enter()
    {
        base.Enter();
        Init();
    }
    public override void Exit()
    {
    }
    void Init()
    {
        canvasGroup.alpha=0;
        canvasGroup.DOFade(1, animatorPersistTime) // 淡入到完全不透明
                    .SetEase(Ease.InOutQuad); // 使用缓入缓出效果

        gemText.text=PlayerDataManager.instance.GetGem().ToString();
        StartCoroutine(AnimateCoinIncrease(currentCoins, PlayerDataManager.instance.GetCoin(), animatorPersistTime)); // 动画持续时间为2秒
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
