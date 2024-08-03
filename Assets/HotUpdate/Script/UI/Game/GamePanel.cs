using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : UIState
{
    public Button pauseButton;

    private void Start()
    {
        pauseButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.PausePanel);
        });
    }

}
