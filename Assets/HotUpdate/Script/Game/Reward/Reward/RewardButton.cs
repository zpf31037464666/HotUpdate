using UnityEngine;
using UnityEngine.UI;
using System;

public class RewardButton : MonoBehaviour
{
    [SerializeField] private Image BG;
    [SerializeField] private Image headImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptText;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            Debug.Log("点击事件");
        });
    }

    public void Init(IRewardable rewardable)
    {
        // 异步加载并显示奖励信息
        rewardable.ReturnRewardInfo((info) =>
        {
            if (info != null)
            {
                BG.sprite = info.bgSprite;
                headImage.sprite = info.headSprite;
                nameText.text = info.name;
                descriptText.text = info.description;
                // 添加按钮点击事件
                button.onClick.AddListener(() => rewardable.GetReward());
            }
            else
            {
                Debug.LogError("Failed to load reward info");
            }
        });
    }

    public void ButtonAddAction(Action action)
    {
        button.onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }
}
