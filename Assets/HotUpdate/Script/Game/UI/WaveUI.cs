using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField] Text timeText;
    [SerializeField] GameObject waveUI;
    Canvas canvas;

    public int waveNumber = 0;

    public static Action OnRewardEvent;
    public static event Action OnGameWin;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    IEnumerator Start()
    {
        while (GameManager.GameState!= GameState.GameOver)
        {
            OpenUI();
            yield return new WaitForSeconds(1f);
            CloseUI();
          yield return  StartCoroutine(RewardCoroutine());
        }
    }

    void OpenUI()
    {
        waveUI.SetActive(true);
        if (EnemyWaveManager.instance.IsLastEnemyWaveData(waveNumber))
        {
            waveUI.GetComponentInChildren<Text>().text = $"----最后一波----";
        }
        else
        {
            waveUI.GetComponentInChildren<Text>().text = $"----第{waveNumber}波----";
        }
        GameManager.GameState= GameState.UI;
    }
    void CloseUI()
    {
        waveUI.SetActive(false);
        GameManager.GameState= GameState.Playing;
    }
    IEnumerator RewardCoroutine()
    {
        var enemyWaveList = EnemyWaveManager.instance.GetEnemyWaveData(waveNumber);
        int time = enemyWaveList[0].RewardTime;
        while (time > 0)
        {
            time -= 1;
            timeText.text = time.ToString();
            yield return new WaitForSeconds(1f);
        }
        if (EnemyWaveManager.instance.IsLastEnemyWaveData(waveNumber))
        {
            GameManager.GameState= GameState.GameOver;
            OnGameWin?.Invoke();
            yield break;
        }


        GameManager.GameState=GameState.Reward;
        OnRewardEvent?.Invoke();


        yield return new WaitUntil(() => GameManager.GameState== GameState.UI);

        waveNumber++;
    }
}