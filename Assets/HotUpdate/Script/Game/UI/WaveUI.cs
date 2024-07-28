using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField] Text timeText;
    [SerializeField] GameObject waveUI;

    [SerializeField] int rewardTime;

    WaitUntil waitGameState;
    Canvas canvas;

    int waveNumber = 1;

    public static Action OnRewardEvent;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        waitGameState = new WaitUntil(() => GameManager.GameState== GameState.Playing);
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
        waveUI.GetComponentInChildren<Text>().text = $"----第{waveNumber}波----";
    }
    void CloseUI()
    {
        waveUI.SetActive(false);
    }
    IEnumerator RewardCoroutine()
    {
        int time = rewardTime;
        while (time > 0)
        {
            timeText.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time -= 1;
        }
        OnRewardEvent?.Invoke();
        yield return waitGameState;
        waveNumber++;
    }
}