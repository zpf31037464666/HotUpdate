using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    void Start()
    {
        button.onClick.AddListener(() =>
        {
            LoadMainScene();
        });
    }
    private void LoadMainScene()
    {
        var handle = Addressables.LoadSceneAsync("Game2", LoadSceneMode.Single);
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
