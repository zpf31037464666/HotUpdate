using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : UIState
{
    [SerializeField] Button resetButton;
    [SerializeField] Button backButton;
    // Start is called before the first frame update
    void Start()
    {
        resetButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.GamePanel);
            SceneLoadManager.instance.LoadScene(GameManager.instance.currentLevelData.ScenceName);
        });
        backButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.MainMenuPanel);
            SceneLoadManager.instance.LoadScene("Scenes/Main.unity", () => UIManager.Instance.SwitchPanel(My_UIConst.MainMenuPanel));

        });
    }

  
}
