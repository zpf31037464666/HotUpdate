using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictorData
{
    public int coinNumber;
    public int genNumber;
}

public class VictorPanel : UIState
{
    [SerializeField] private Text coinText;
    [SerializeField] private Text gemText;
    [SerializeField] private Button comfimButton;

    void Start()
    {
        comfimButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.MainMenuPanel);
            GameManager.GameState=GameState.Playing;
        });
    }
    public override void Enter()
    {
        base.Enter();

        LevelManager.instance.UnlockLevel(GameManager.instance.currentLevelData.UnLockNextLevel);
    }
    private void SetInfo(VictorData data)
    {
        coinText.text=data.coinNumber.ToString();
        gemText.text=data.genNumber.ToString();

    }
    public override void SetData(Dictionary<string, object> data)
    {
        if (data.ContainsKey("VictorData") && data["VictorData"] is VictorData info)
        {
            SetInfo(info);
        }
    }

}
