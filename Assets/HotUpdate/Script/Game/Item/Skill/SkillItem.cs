using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillItem : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Image coolDownImage;
    [SerializeField] TMP_Text coolDownText;
    [SerializeField] Image disableImage;


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
    public void UpdateCoolDownText(float amount)
    {
        if(amount<=0)
        {
            coolDownText.gameObject.SetActive(false);
        }
        else
        {
            coolDownText.gameObject.SetActive(true);
        }
        coolDownText.text = amount.ToString("0");
    }

    public void DisableImage(bool disable)
    {
        disableImage.gameObject.SetActive(disable);
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
