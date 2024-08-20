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

    private List<EnemyWaveData> currentEnemyWaveData = new List<EnemyWaveData>();

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        Debug.Log(GameManager.instance.currentSelectSenceName);

        currentEnemyWaveData=EnemyWaveManager.instance.GetEnemyWaveData(GameManager.instance.currentSelectSenceName);

        if(currentEnemyWaveData == null)
        {
            Debug.Log("currentEnemyWaveData Null");

        }
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
     //  if (EnemyWaveManager.instance.IsLastEnemyWaveData(waveNumber))
        if (IsLastEnemyWaveData(waveNumber))
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
       // var enemyWaveList = EnemyWaveManager.instance.GetEnemyWaveData(waveNumber);
        var enemyWaveList = GetEnemyWaveData(waveNumber);
        int time = enemyWaveList[0].RewardTime;
        while (time > 0)
        {
            time -= 1;
            timeText.text = time.ToString();
            yield return new WaitForSeconds(1f);
        }
        //if (EnemyWaveManager.instance.IsLastEnemyWaveData(waveNumber))
        if (IsLastEnemyWaveData(waveNumber))
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

    public List<EnemyWaveData> GetEnemyWaveData(int currentWave)
    {
        List<EnemyWaveData> currentWaves = currentEnemyWaveData.FindAll(w => w.WaveNumber == currentWave);

        // 如果没有找到，返回一个空的列表
        if (currentWaves.Count == 0)
        {
            Debug.LogWarning($"No enemy wave data found for wave number: {currentWave}");
            return null;
        }

        return currentWaves; // 返回找到的波数据（可能是空列表）
    }
    //判断是否为最后一波
    public bool IsLastEnemyWaveData(int currentWave)
    {
        List<EnemyWaveData> currentWaves = currentEnemyWaveData.FindAll(w => w.WaveNumber == currentWave);
        if (currentWaves.Count == 0)
        {
            Debug.LogWarning($"No enemy wave data found for wave number: {currentWave}");
            return false;
        }
        return currentWaves[0].IsOverGame;

    }
}