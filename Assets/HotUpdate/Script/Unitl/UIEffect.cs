using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEffect
{
    private static UIEffect instance;
    public static UIEffect Instance
    {
        get
        {
            // 如果实例尚未被创建，则创建一个新的实例
            if (instance == null)
            {
                instance = new UIEffect();
            }
            return instance;
        }
    }
    public void SetUIMove(GameObject UI, Vector3 moveMent, float duration, Action action = null)
    {
        UI.transform.DOMove(moveMent, duration).OnComplete(() => action?.Invoke());
    }
    public void SetUIZoom(GameObject UI, float zoom, float duration, Action action = null)
    {
        UI.transform.DOScale(zoom, duration).SetEase(Ease.Linear)
            .OnComplete(() => action?.Invoke());
    }
    public void SetUIFabe(Image image, float duration, bool isvision = true, Action action = null)
    {
        if (isvision)
        {
            image.DOFade(1f, duration).OnComplete(() => action?.Invoke());
        }
        else
        {
            image.DOFade(0f, duration).OnComplete(() => action?.Invoke());
        }

    }
    public void SetTextFabe(Text text, float duration, Action action = null)
    {
        text.DOFade(1f, duration).OnComplete(() => action?.Invoke());
    }
}
