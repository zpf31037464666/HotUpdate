using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private Text messageText;

    RectTransform rectTransform;

    public float moveDuration = 1f; // 移动持续时间
    private void Awake()
    {
        rectTransform=GetComponent<RectTransform>();
    }

    public void ShowMessage(string messge, Action action=null)
    {

        Original();
        float screenHeight = Screen.height;
        Vector2 targetPosition = new Vector2(rectTransform.anchoredPosition.x, screenHeight/2);

        // 将 UI 元素移动到目标位置，并添加回弹效果
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => messageText.text =messge);
        s.Append(rectTransform.DOAnchorPos(targetPosition, moveDuration/2, true).SetEase(Ease.OutCubic));
        //延迟调用函数
        s.Append(DOVirtual.DelayedCall(moveDuration/2, Hide));
        s.AppendCallback(() => action?.Invoke());

    }

    void Hide()
    {
        bg.DOFade(0f, 0.2f).SetEase(Ease.OutCubic);
        messageText.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            ObjectPool.Instance.PushObject(gameObject);
        });
    }
    void Original()
    {
        Debug.Log("恢复原样");
        bg.DOFade(1, 0.1f).SetEase(Ease.OutCubic);
        messageText.DOFade(1, 0.1f).SetEase(Ease.OutCubic);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 700); // 初始位置在屏幕上方
    }
}
