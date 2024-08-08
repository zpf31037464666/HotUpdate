using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemyWaveManager : Singleton<EnemyWaveManager>
{
    private List<EnemyWaveData> enemyWaveList = new List<EnemyWaveData>();

    private void Start()
    {
        LoadJson();
    }
    private void LoadJson()
    {
        // 使用 Addressables 异步加载 JSON 文件
        Addressables.LoadAssetAsync<TextAsset>("Data/EnemyWave.json").Completed += OnJsonLoaded;
    }
    private void OnJsonLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // JSON 文件加载成功
            TextAsset jsonAsset = handle.Result;
            string json = jsonAsset.text;
            enemyWaveList = JsonConvert.DeserializeObject<List<EnemyWaveData>>(json);
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + handle);
        }
    }

    public List<EnemyWaveData> GetEnemyWaveData(int currentWave)
    {
        List<EnemyWaveData> currentWaves = enemyWaveList.FindAll(w => w.WaveNumber == currentWave);

        // 如果没有找到，返回一个空的列表
        if (currentWaves.Count == 0)
        {
            Debug.LogWarning($"No enemy wave data found for wave number: {currentWave}");
            return null;
        }

        return currentWaves; // 返回找到的波数据（可能是空列表）
    }
    public bool IsBeyond(int waveNumber) => enemyWaveList.Count<=waveNumber;

}
