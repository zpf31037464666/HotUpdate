using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] RewardButton rewardButton;
    [SerializeField] Transform rewardGroup;

    private int rewardNumber = 3;

    private void OnEnable()
    {
        WaveUI.OnRewardEvent+=onRewardEvent;
    }
    private void OnDisable()
    {
        WaveUI.OnRewardEvent-=onRewardEvent;
    }

    private void onRewardEvent()
    {
        Debug.Log("调用奖励");

        GameManager.GameState=GameState.Reward;
        CreateReward();
    }

    void ClearReward()
    {
        foreach (Transform child in rewardGroup)
        {
            Destroy(child.gameObject);
        }
    }
    public void CreateReward()
    {
        canvas.enabled=true;
        ClearReward();
        IRewardable[] rewardables = RewardManager.instance.GetRewardble(rewardNumber);
        for (int i = 0; i < rewardables.Length; i++)
        {
            var rewardObject = Instantiate(rewardButton, rewardGroup);
            rewardObject.GetComponent<RewardButton>().Init(rewardables[i]);
            rewardObject.GetComponent<RewardButton>().ButtonAddAction(
                () => 
                    {
                        canvas.enabled=false;
                        GameManager.GameState=GameState.Playing;
                    } 
            );
         }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("生成奖励");
            CreateReward();
        }
    }
}
