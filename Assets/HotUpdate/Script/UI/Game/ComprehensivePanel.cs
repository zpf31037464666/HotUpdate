using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComprehensivePanel : UIState
{
    [SerializeField] Button gamePlayButton;
    [SerializeField] Button taskPaneButton;

    private void Start()
    {
        gamePlayButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.SelectPlayerPanel);
        });
        taskPaneButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.TaskPanel);
        });
    }

}
