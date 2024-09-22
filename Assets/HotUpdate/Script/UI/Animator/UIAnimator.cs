using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(LayoutElement))]
public class UIAnimator : MonoBehaviour
{
    private LayoutElement element;
    private void Awake()
    {
        element = GetComponent<LayoutElement>();
    }
    public void StretchToWidth(float initWidth, float targetWidth, float time)
    {
        element.preferredWidth=initWidth;
        DOTween.To(() => element.preferredWidth, x => element.preferredWidth=x, targetWidth, time);

    }
}
