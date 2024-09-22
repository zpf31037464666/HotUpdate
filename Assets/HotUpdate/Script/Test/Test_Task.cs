using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Task : MonoBehaviour,IInitializable<TaskInfo>
{
    [SerializeField] private Text nameText;
    [SerializeField] private Image iconImage;

    public void Init(TaskInfo item)
    {
        nameText.text = item.name;
        iconImage.sprite=item.iconSprite;
    }
}
