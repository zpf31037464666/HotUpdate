using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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
    [SerializeField] private Image bgImage;
    [SerializeField] private Image lockImage;

    private LevelSelectPanel levelSelector;
    private Button levelButton;

    // private LevelInfo levelInfo;
    private LevelData levelData;

    private float doubleTime = .5f;
    bool isDouble=false;
    
    private void Awake()
    {
        levelButton=GetComponent<Button>();
    }
    public void RegisterLevelSelector(LevelSelectPanel levelSelector, LevelData levelData)
    {
        levelButton.onClick.RemoveAllListeners();

        this.levelSelector = levelSelector;
        levelButton.onClick.AddListener(() =>
        {
            levelSelector.MoveLevelToBottom(int.Parse(gameObject.name));
            OnclickEvent();
        });

        SetLevelInfo(levelData);
    }
    private void SetLevelInfo(LevelData levelData)
    {
        this.levelData = levelData;
        nameText.text = levelData.Name;
        lockImage.gameObject.SetActive(!levelData.IsUnLock);
        Addressables.LoadAssetAsync<Sprite>(levelData.IconPath).Completed += OnLoadSpite;
        Addressables.LoadAssetAsync<Sprite>(levelData.BGPath).Completed+=(handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                bgImage.sprite= handle.Result;
            }
        };
    }

    private void OnLoadSpite(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            headImage.sprite= handle.Result;
        }
    }

    private void OnclickEvent()
    {
        if (!levelData.IsUnLock) return;
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
        SceneLoadManager.instance.LoadScene(levelData.ScenceName, My_UIConst.GamePanel, levelSelector.moveDuration*2);

        GameManager.instance.currentSelectSenceName = levelData.Name;
    }
}
