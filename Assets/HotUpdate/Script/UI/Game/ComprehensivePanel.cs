using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using UnityEngine.UI;

public class ComprehensivePanel : UIState
{
    [SerializeField] Button gamePlayButton;
    [SerializeField] Button taskPaneButton;
    [SerializeField] Button shopPaneButton;


    [SerializeField] Text coinText;
    [SerializeField] Text gemText;
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
        shopPaneButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.ShopPanel);
        });
    }
    public override void Enter()
    {
        base.Enter();
        Init();
    }
    void Init()
    {
        coinText.text=PlayerDataManager.instance.GetCoin().ToString();
        gemText.text=PlayerDataManager.instance.GetGem().ToString();
    }

}
