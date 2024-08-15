using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIManager : StateMachina
{
    public static UIManager Instance;

    private Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();
    private Stack<string> panelHistory = new Stack<string>(); // 用于存储面板历史记录

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

       SwitchPanel(My_UIConst.MainMenuPanel);
    }

    public void CreatePanel(string name, System.Action<GameObject> onLoaded)
    {
        if (prefabDict.ContainsKey(name))
        {
            Debug.Log("包含物体" + name);
            onLoaded?.Invoke(prefabDict[name]);
            return;
        }

        Addressables.LoadAssetAsync<GameObject>(name).Completed += handle =>
        {
            OnPanelLoaded(handle, onLoaded, name);
        };
    }

    private void OnPanelLoaded(AsyncOperationHandle<GameObject> handle, System.Action<GameObject> onLoaded, string name)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject panelPrefab = handle.Result;
            GameObject panelObject = Instantiate(panelPrefab, gameObject.transform, false);

            prefabDict[name] = panelObject;
            onLoaded?.Invoke(panelObject); // 调用回调，返回实例化的面板对象
        }
        else
        {
            Debug.LogError($"Failed to load panel prefab: {handle}");
            onLoaded?.Invoke(null); // 加载失败时返回 null
        }
    }

    public void SwitchPanel(string name)
    {
        CreatePanel(name, panelObject =>
        {
            if (panelObject != null)
            {
                panelHistory.Push(name); 

                UIState newState = panelObject.GetComponent<UIState>();
                SwitchState(newState);
                Debug.Log("Switched to panel: " + name);
            }
            else
            {
                Debug.LogError($"Failed to create panel: {name}");
            }
        });
    }
    public void ReturnToPreviousPanel()
    {
        if (panelHistory.Count > 1) // 确保至少有两个面板（当前面板和上一个面板）
        {
            panelHistory.Pop(); // 弹出当前面板
            string previousPanelName = panelHistory.Pop(); // 从栈中弹出上一个面板名称
            SwitchPanel(previousPanelName); // 切换到上一个面板
        }
        else
        {
            Debug.LogWarning("No previous panel to return to.");
        }
    }

}
