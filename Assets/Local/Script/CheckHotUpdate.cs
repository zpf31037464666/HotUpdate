using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 确保包含 UI 命名空间

public class CheckHotUpdate : MonoBehaviour
{
    public Image precentImage;
    public Text downloadInfoText; // 用于显示下载信息的 UI 文本

    bool isFinal=false;

    void Start()
    {
        StartCoroutine(FetchRomoteLabelDownloadSize());
    }
    private void Update()
    {
        if (isFinal&&Input.GetKeyDown(KeyCode.Space))
        {
            LoadMainScene();
        }
        if (isFinal)
        {
            // 检测是否有触摸输入
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                // 检测触摸状态是否为已按下
                if (touch.phase == TouchPhase.Began)
                {
                    // 触摸屏幕时的逻辑
                    Debug.Log("屏幕被点击!");
                    LoadMainScene();
                }
            }
        }
    }


    IEnumerator FetchRomoteLabelDownloadSize()
    {
        AsyncOperationHandle<long> downloadSize = Addressables.GetDownloadSizeAsync("HotUpdate");
        yield return downloadSize;

        if (downloadSize.Status == AsyncOperationStatus.Succeeded)
        {
            if (downloadSize.Result <= 0)
            {
                Debug.Log("没有更新");
                EnterGame();
            }
            else
            {
                Debug.Log($"有更新，预计下载大小: {downloadSize.Result / (1024f * 1024f):F2} MB");
                StartCoroutine(DownLoadDependencies(downloadSize.Result));
            }
        }
        else
        {
            Debug.LogError("获取下载大小失败: ");
        }
        Addressables.Release(downloadSize);
    }

    private IEnumerator DownLoadDependencies(long totalSize)
    {
        AsyncOperationHandle handle = Addressables.DownloadDependenciesAsync("HotUpdate");
        while (!handle.IsDone)
        {
            var downloadStatus = handle.GetDownloadStatus();
            var downloadedBytes = downloadStatus.DownloadedBytes;
            var totalBytes = downloadStatus.TotalBytes;

            float progress = downloadStatus.Percent;
            float downloadedMB = downloadedBytes / (1024f * 1024f);
            float totalMB = totalBytes / (1024f * 1024f);

            // 更新 UI 文本显示下载信息
            precentImage.fillAmount=progress;
            downloadInfoText.text = $"{downloadedMB:F2} MB / {totalMB:F2} MB ({progress * 100:F2}%)";
            yield return null;
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Addressables.Release(handle);
            EnterGame();
        }
        else
        {
            Debug.LogError("下载依赖项失败: " + handle);
        }
    }

    private async void EnterGame()
    {
        UpdateFinalUI();

        var loadDall = Addressables.LoadAssetAsync<TextAsset>("HotUpdate.dll");
        await loadDall.Task;
        Assembly hotUpdate = Assembly.Load(loadDall.Result.bytes);
        isFinal=true;
    }

    private void UpdateFinalUI()
    {
        precentImage.fillAmount=1;
        downloadInfoText.text ="完成下载";
    }


    private void LoadMainScene()
    {
        var handle = Addressables.LoadSceneAsync("Scenes/Main.unity", LoadSceneMode.Single);
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
