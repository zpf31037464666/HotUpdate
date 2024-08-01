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
            SceneLoadManager.instance.LoadScene("Game2");
            canvas.enabled=false;
        });
        optionButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.OptionPanel);
        });
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
    public override void Enter()
    {
        Debug.Log("进入主菜单");
        titleText.text ="武器大师";

        base.Enter();
    }

    public override void Exit()
    {

        base.Exit();
    }


}
