using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWinUI : MonoBehaviour
{
    public int unLockId;

    [SerializeField] Button restButton;
    [SerializeField] Button exitButton;
    Canvas canvas;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    private void Start()
    {
        restButton.onClick.AddListener(() =>
        {
            LevelManager.instance.UnlockLevel(unLockId);
          //  SceneLoadManager.instance.LoadScene("Scenes/Main.unity", My_UIConst.MainMenuPanel, .5f);

            SceneLoadManager.instance.LoadScene("Scenes/Main.unity");
            UIManager.Instance.SwitchPanel(My_UIConst.MainMenuPanel);

            GameManager.GameState=GameState.Playing;

        });

        exitButton.onClick.AddListener(() =>
        {
            LevelManager.instance.UnlockLevel(unLockId);
            SceneLoadManager.instance.LoadScene("Scenes/Main.unity", My_UIConst.MainMenuPanel, .5f);

            GameManager.GameState=GameState.Playing;
        });
    }
    private void OnEnable()
    {
        WaveUI.OnGameWin+=GameWin;
    }
    private void OnDisable()
    {
        WaveUI.OnGameWin-=GameWin;
    }

    private void GameWin()
    {
        Open();
        //解锁场景
        LevelManager.instance.UnlockLevel(unLockId);
    }

    void Open()
    {
        canvas.enabled=true;
    }

}
