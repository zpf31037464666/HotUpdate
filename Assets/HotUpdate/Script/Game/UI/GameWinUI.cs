using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWinUI : MonoBehaviour
{
    [SerializeField] Button restButton;
    [SerializeField] Button exitButton;
    Canvas canvas;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
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
    }

    void Open()
    {
        canvas.enabled=true;
    }

}
