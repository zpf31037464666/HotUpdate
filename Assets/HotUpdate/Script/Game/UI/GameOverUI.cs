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
            //测试
            SceneLoadManager.instance.LoadScene("Scenes/Main.unity", () => UIManager.Instance.SwitchPanel(My_UIConst.MainMenuPanel));
        });
        exitButton.onClick.AddListener(() =>
        {
            //临时
            // ReloadCurrentScene();
            SceneLoadManager.instance.LoadScene("Scenes/Main.unity", ()=>UIManager.Instance.SwitchPanel(My_UIConst.MainMenuPanel));

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
}
