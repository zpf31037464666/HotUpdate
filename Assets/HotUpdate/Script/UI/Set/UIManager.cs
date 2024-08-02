using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIManager : StateMachina
{
    public static UIManager Instance;

    private Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();
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
            Debug.Log("包含物体"+name);
            onLoaded?.Invoke(prefabDict[name]);
            return;
        }

        Addressables.LoadAssetAsync<GameObject>(name).Completed += handle =>
        {
            OnPanelLoaded(handle, onLoaded,name);
        };
    }

    private void OnPanelLoaded(AsyncOperationHandle<GameObject> handle, System.Action<GameObject> onLoaded,string name)
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
}
