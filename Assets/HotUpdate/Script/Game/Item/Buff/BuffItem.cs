using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuffItem : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Image coolImage;
    [SerializeField] TMP_Text stackText;

    public void SetIconImage(Sprite icon)
    {
        iconImage.sprite = icon;
    }
    public void UpdateCoolTime(float percentage)
    {
        coolImage.fillAmount = percentage;
    }
    public void UpdateStackText(int stack)
    {
        stackText.text=stack.ToString();
    }
}
