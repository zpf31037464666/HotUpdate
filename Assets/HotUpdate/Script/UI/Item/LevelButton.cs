using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelInfo
{
    public int level;
    public string name;
    public string description;
    public Sprite headSprite;
    public bool isLock;
}

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Image headImage;

    private LevelSelectPanel levelSelector;
    private Button levelButton;
    private LevelInfo levelInfo;

    private float doubleTime = .5f;
    bool isDouble=false;
    
    private void Awake()
    {
        levelButton=GetComponent<Button>();
    }
    public void RegisterLevelSelector(LevelSelectPanel levelSelector, LevelInfo levelInfo)
    {
        this.levelSelector = levelSelector;
        levelButton.onClick.AddListener(() =>
        {
            levelSelector.MoveLevelToBottom(int.Parse(gameObject.name));
            OnclickEvent();
        });

        SetLevelInfo(levelInfo);
    }
    private void SetLevelInfo(LevelInfo levelInfo)
    {
        this.levelInfo = levelInfo;

        headImage.sprite=levelInfo.headSprite;
        nameText.text = levelInfo.name;
    }
    private void OnclickEvent()
    {
        if(isDouble)
        {
            Effcet();
            return;
        }
        StartCoroutine(DoubleCoroutine());
    }
    IEnumerator DoubleCoroutine()
    {
        isDouble=true;
        yield return new WaitForSeconds(doubleTime);
        isDouble=false;
    }
    public void Effcet()
    {
        Debug.Log("双击"+levelInfo.name);

        SceneLoadManager.instance.LoadScene(levelInfo.level.ToString(), My_UIConst.GamePanel, levelSelector.moveDuration*2);
       // UIManager.Instance.SwitchPanel(My_UIConst.GamePanel);
    }
}
