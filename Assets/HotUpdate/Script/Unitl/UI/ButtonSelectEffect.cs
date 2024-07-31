using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] List<Image> imageList;
    [SerializeField] AudioData enterAduioClip;
    [SerializeField] AudioData exitAduioClip;
    [SerializeField] AudioData submitAduioClip;

    public float targetAlpha = 1f; // 目标透明度
    public float originAlpha = 0f; // 目标透明度
    public float duration = 1f; // 动画持续时间

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayRandomSFXaudio(submitAduioClip);
        });
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlayRandomSFXaudio(enterAduioClip);

        foreach (var item in imageList)
        {
            item.DOFade(targetAlpha, duration);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AudioManager.instance.PlayRandomSFXaudio(exitAduioClip);
        foreach (var item in imageList)
        {
            item.DOFade(originAlpha, duration);
        }
    }

}
