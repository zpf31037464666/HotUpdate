using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 确保包含 UI 命名空间


public class Main : MonoBehaviour
{
    [Header("test")]
    [SerializeField] Text nameText;
    [SerializeField] Button nextLevelButton;
    [SerializeField] Button exitButton;

    private void OnEnable()
    {
        nameText.text="更新";
    }
    private void Start()
    {
        nextLevelButton.onClick.AddListener(() =>
        {
            LoadMainScene();
        });
        exitButton.onClick.AddListener(() =>
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false; // 在编辑器中停止播放模式
            #else
                    Application.Quit(); // 在构建版本中退出应用
            #endif
                        Debug.Log("应用程序已关闭");
        });
    }

    private void LoadMainScene()
    {
        var handle = Addressables.LoadSceneAsync("Level1", LoadSceneMode.Single);
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
