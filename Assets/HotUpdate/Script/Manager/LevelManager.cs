using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelManager : PersistentSingleton<LevelManager>
{
    public List<LevelData> levelDataList = new List<LevelData>();
    private void Start()
    {
        LoadJson();
    }
    private void LoadJson()
    {
        // 使用 Addressables 异步加载 JSON 文件
        Addressables.LoadAssetAsync<TextAsset>("LevelData").Completed += OnJsonLoaded;
    }
    private void OnJsonLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // JSON 文件加载成功
            TextAsset jsonAsset = handle.Result;
            string json = jsonAsset.text;
            levelDataList = JsonConvert.DeserializeObject<List<LevelData>>(json);
            foreach (var level in levelDataList)
            {
               Debug.Log(level.Name);
            }
            // 在这里处理 JSON 数据
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + handle);
        }
    }
}
