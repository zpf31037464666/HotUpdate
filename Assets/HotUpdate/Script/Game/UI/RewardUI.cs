using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] RewardButton rewardButton;

    public Transform cardParent;
    public Transform cardStartPosT;
    public int cardCount = 10;
    public float cardWidth = 1f;

    public float dealDuration = 1f;
    public float delayBetweenCards = 0.2f;

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
        CreateCard();
    }
    void CreateCard()
    {
        canvas.enabled=true;

        foreach (Transform item in cardParent)
        {
            Destroy(item.gameObject);
        }

        IRewardable[] rewardables = RewardManager.instance.GetOwerRewardable(cardCount);

        int cardNumber =cardCount;

        float totalWidth = cardNumber * cardWidth;
        float startingX = cardParent.position.x - totalWidth / 2 + cardWidth / 2;
        for (int i = 0; i < cardNumber; i++)
        {
            var card = Instantiate(rewardButton);

            // Position the card
            float cardX = startingX + i * cardWidth;
            card.transform.position = cardStartPosT.position;

            card.transform.SetParent(cardParent);
            card.transform.localScale = Vector3.one;


            DealCard(card.gameObject, i * delayBetweenCards, new Vector3(cardX, cardParent.position.y, cardParent.position.z), rewardables[i]);
        }

    }
    private void DealCard(GameObject card, float delay, Vector3 targetPos, IRewardable rewardable)
    {
        // Store the initial rotation
        Quaternion initialRotation = card.transform.rotation;

        // Create rotate animation
        var sequence = DOTween.Sequence();

        sequence.AppendInterval(delay);

        // Move card to target position
        sequence.Append(card.transform.DOMove(targetPos, 0.1f));

        // Rotate card to half way over dealDuration / 2
        sequence.Append(card.transform.DORotate(new Vector3(0, 90, 0), dealDuration / 2));

        // Add a function callback here to do something when the card is half way turned
        // For example, change the card's sprite to show its face
        sequence.AppendCallback(() => HalfWayThere(card, rewardable));

        // Rotate card to fully turned over dealDuration / 2
        sequence.Append(card.transform.DORotate(initialRotation.eulerAngles, dealDuration / 2));

        // Add a callback for completion after the animation finishes
        //  sequence.OnComplete(() => ResetReawardPlane());

    }

    private void HalfWayThere(GameObject card, IRewardable rewardable)
    {
        card.GetComponent<RewardButton>().Init(rewardable);
        card.GetComponent<RewardButton>().ButtonAddAction(
            () =>
            {
                canvas.enabled=false;
                GameManager.GameState=GameState.UI;
            }
        );

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("生成奖励");
            //  CreateReward();
            CreateCard();
        }
    }
}
