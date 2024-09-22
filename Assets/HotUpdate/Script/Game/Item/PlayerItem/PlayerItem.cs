using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private Image bgimage;
    [SerializeField] private Image headImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private Text nameText;

    private PlayerItemData data;
    private SelectPlayerPanel selcetPanel;

    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Init(PlayerItemData data, SelectPlayerPanel selectPlayerPanel)
    {
        this.data = data;
        this.selcetPanel = selectPlayerPanel;

        SetPlayerItemInfo(data);

        button.onClick.AddListener(() =>
        {
            selectPlayerPanel.SetShowStage(data.IsUnLock, bgimage.sprite, headImage.sprite, data.Name, data.Description, data);
        });

    }
    public void SelcetShowStaet()
    {
        selcetPanel.SetShowStage(data.IsUnLock, bgimage.sprite, headImage.sprite, data.Name, data.Description, data);
    }
    private void SetPlayerItemInfo(PlayerItemData data)
    {
        nameText.text = data.Name;
        lockImage.gameObject.SetActive(!data.IsUnLock);
        Addressables.LoadAssetAsync<Sprite>(data.BGPath).Completed+=(handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                bgimage.sprite= handle.Result;
            }
        };
        Addressables.LoadAssetAsync<Sprite>(data.IconPath).Completed+=(handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                headImage.sprite= handle.Result;
            }
        };
    }
}
