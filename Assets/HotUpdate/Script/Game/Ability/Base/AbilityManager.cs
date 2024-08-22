using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class AbilityManager:PersistentSingleton<AbilityManager>
{
    public List<AbilityData> abilityDataList = new List<AbilityData>();
    public List<Ability> abilitieList = new List<Ability>();

    private void Start()
    {
        LoadJson();
        foreach (string name in AbilityFactory.GetNames())
        {
            Debug.Log("task 具体"+   name);
        }
    }
    private void LoadJson()
    {
        // 使用 Addressables 异步加载 JSON 文件
        Addressables.LoadAssetAsync<TextAsset>("Data/abilityData.json").Completed += OnJsonLoaded;
    }
    private void OnJsonLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // JSON 文件加载成功
            TextAsset jsonAsset = handle.Result;
            string json = jsonAsset.text;
            abilityDataList = JsonConvert.DeserializeObject<List<AbilityData>>(json);
            foreach (var data in abilityDataList)
            {
                GrantAbility(data.Id);
            }
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + handle);
        }
    }
    private void GrantAbility(int id)
    {
        var data = abilityDataList.Find(r => r.Id == id);
        Ability ability = AbilityFactory.GetAblility(data.Type, data);
        if (!abilitieList.Contains(ability))
        {
            abilitieList.Add(ability);
        }
    }

    public Ability GetAbility(int id)
    {
        foreach (var ability in abilitieList)
        {
            if(ability.data.Id == id)
            {
                return ability;
            }
        }
        Debug.Log("ability Null");
        return null;
    }
    public Ability GetAbility(string name)
    {
        foreach (var ability in abilitieList)
        {
            if (ability.data.Name == name)
            {
                return ability;
            }
        }
        Debug.Log("ability Null");
        return null;
    }
}

