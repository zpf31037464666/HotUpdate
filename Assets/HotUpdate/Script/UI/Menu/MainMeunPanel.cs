using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMeunPanel : UIState
{
    public Text titleText;
    public Button startButton;
    public Button optionButton;
    public Button exitButton;

    public Image bg;
    private void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.ComprehensivePanel);
        });
        optionButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.OptionPanel);
        });
        exitButton.onClick.AddListener(() =>
        {
                #if UNITY_EDITOR
                          UnityEditor.EditorApplication.isPlaying = false; // 在编辑器中停止播放模式
                #else
                          Application.Quit(); // 在构建版本中退出应用
                #endif
        });
    }
    public override void Enter()
    {
        titleText.text ="爱你默默的";

        base.Enter();
    }

    public override void Exit()
    {

        base.Exit();
    }


}
