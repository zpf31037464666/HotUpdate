using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : UIState
{
    public Button contineButton;
    public Button optionButton;
    public Button exitButton;
    // Start is called before the first frame update
    private float moveDuration = 0.5f;
    void Start()
    {
        contineButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.GamePanel);
        });
        optionButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.OptionPanel);
        });
        exitButton.onClick.AddListener(() =>
        {
            SceneLoadManager.instance.LoadScene("Scenes/Main.unity");
            UIManager.Instance.SwitchPanel(My_UIConst.MainMenuPanel);
            //SceneLoadManager.instance.LoadScene("Main", My_UIConst.MainMenuPanel, moveDuration);
        });
    }
    public override void Enter()
    {
        base.Enter();
        canvas.sortingOrder=99;
        Time.timeScale=0f;
    }
    public override void Exit()
    {
        Time.timeScale=1f;
        canvas.sortingOrder=0;
        base.Exit();
        //rectTransform.DOScale(Vector3.one*1.2f, moveDuration).OnComplete(()=>
        //{
        //    canvas.sortingOrder=0;
        //    canvas.enabled=false;
        //});
    }

}
