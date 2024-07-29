using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : PersistentSingleton<GameManager>
{
    public static Action onGameOver;
    //设置属性 其他类可以访问和设置
    public static GameState GameState { get => instance.gameState; set => instance.gameState = value; }
    [SerializeField] GameState gameState = GameState.UI;
}
public enum GameState
{
    Playing,
    Timeline,
    Paused,
    GameOver,
    Reward,
    UI
}