using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Button nextLevelButton;

    private void OnEnable()
    {
        nameText.text="热更新";
    }
    private void Start()
    {
        nextLevelButton.onClick.AddListener(() =>
        {
            LoadMainScene();
        });
    }

    private void LoadMainScene()
    {
        var handle = Addressables.LoadSceneAsync("Main", LoadSceneMode.Single);
        handle.Completed += OnSenceLoad;
    }

    private void OnSenceLoad(AsyncOperationHandle<SceneInstance> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("加载成功");
        }
    }
}
