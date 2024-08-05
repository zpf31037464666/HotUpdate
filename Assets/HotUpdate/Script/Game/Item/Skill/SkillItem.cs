using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Image coolDownImage;

    private Button button;
    private Action action=null;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetIcon(Sprite icon)
    {
        iconImage.sprite = icon;
    }
    public void UpdateCoolDown(float precent)
    {
        coolDownImage.fillAmount = precent;
    }
    public void SetAction(Action action)
    {
        this.action = action;

        button.onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }
   
}
