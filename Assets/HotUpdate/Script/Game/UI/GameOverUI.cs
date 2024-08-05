using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 确保包含 UI 命名空间

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Button restButton;
    [SerializeField] Button exitButton;

    Canvas canvas;

    // 用于存储当前场景的地址
    private string currentSceneAddress;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    private void Start()
    {
        // 获取当前场景的名称，设置为 Addressable 地址
        currentSceneAddress = SceneManager.GetActiveScene().name;

        restButton.onClick.AddListener(() =>
        {
            ReloadCurrentScene();
        });
        exitButton.onClick.AddListener(() =>
        {
            //临时
            // ReloadCurrentScene();
            SceneLoadManager.instance.LoadScene("Main", My_UIConst.MainMenuPanel,.5f);

        });
    }



    private void OnEnable()
    {
        Player.OnPlayerDeathEvent+=onPlayerDeathEvent;
    }
    private void OnDisable()
    {
        Player.OnPlayerDeathEvent-=onPlayerDeathEvent;
    }

    private void onPlayerDeathEvent()
    {
        Open();
    }
    void Open()
    {
        canvas.enabled=true;
    }


    public void ReloadCurrentScene()
    {
        StartCoroutine(ReloadSceneCoroutine());
    }
    private IEnumerator ReloadSceneCoroutine()
    {
        // 卸载当前场景
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentSceneAddress);
        yield return unloadOperation;

        // 重新加载场景
        AsyncOperationHandle<SceneInstance> loadOperation = Addressables.LoadSceneAsync(currentSceneAddress);
        yield return loadOperation;

        // 检查加载是否成功
        if (loadOperation.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Scene {currentSceneAddress} reloaded successfully.");
        }
        else
        {
            Debug.LogError($"Failed to reload scene {currentSceneAddress}.");
        }
    }
}
