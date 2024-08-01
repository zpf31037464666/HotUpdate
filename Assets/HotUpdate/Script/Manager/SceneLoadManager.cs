using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoadManager : PersistentSingleton<SceneLoadManager>
{
    public GameObject loadingScreen; // 加载界面
    public Slider progressBar; // 进度条
    public Text progressText; // 显示进度的文本
    public Animator loadingAnimator; // 用于加载动画的 Animator


    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // 显示加载界面
        loadingScreen.SetActive(true);
        loadingAnimator.SetTrigger("StartLoading");

        // 异步加载场景
        var asyncLoad = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        if (asyncLoad.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError("场景加载异常: " + asyncLoad.OperationException.ToString());
            yield break;
        }

        // 更新加载进度
        while (!asyncLoad.IsDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.PercentComplete); // 使用 PercentComplete 获取进度
            progressBar.value = progress;
            progressText.text = (progress * 100).ToString("F0") + "%"; // 显示百分比
            yield return null; // 等待下一帧
        }
        Debug.Log("场景加载完毕");

        // 加载完成后的处理
        loadingAnimator.SetTrigger("EndLoading");
        yield return new WaitForSeconds(1f); // 等待动画播放完成
        loadingScreen.SetActive(false); // 隐藏加载界面
    }
}
