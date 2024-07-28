using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class RewardManager :MonoBehaviour
{
    private List<Reward> rewardsList = new List<Reward>();
    public static RewardManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //    LoadRewards();
        LoadJson();
        foreach (string name in RewardFactory.GetRewardNames())
        {
            Debug.Log("奖励名字"+   name);
        }
    }

    private void LoadJson()
    {
        // 使用 Addressables 异步加载 JSON 文件
        Addressables.LoadAssetAsync<TextAsset>("Reward").Completed += OnJsonLoaded;
    }

    private void OnJsonLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // JSON 文件加载成功
            TextAsset jsonAsset = handle.Result;
            string json = jsonAsset.text;
            rewardsList = JsonConvert.DeserializeObject<List<Reward>>(json);
            foreach (var reward in rewardsList)
            {
                GrantReward(reward.RewardID);
            }
            // 在这里处理 JSON 数据
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + handle);
        }
    }
    public void GrantReward(int rewardID)
    {
        Reward reward = rewardsList.Find(r => r.RewardID == rewardID);

        IRewardable reawd = RewardFactory.GetReward(reward.RewardType, reward);

        Resiter(reawd);
    }

    private List<IRewardable> rewardables = new List<IRewardable>();
    public void Resiter(IRewardable rewardable)
    {
        if (!rewardables.Contains(rewardable))
        {
            Debug.Log(rewardable.Name + " 注册了奖励");
            rewardables.Add(rewardable);
        }
    }
    public void Unregister(IRewardable rewardable)
    {
        if (rewardables.Contains(rewardable))
        {
            Debug.Log(rewardable.Name + " 注消了奖励");
            rewardables.Remove(rewardable);
        }
    }
    public IRewardable[] GetRewardble(int number)
    {
        IRewardable[] newRewards = new IRewardable[number];
        var shuffleRewards = Utilities.ShuffleList(rewardables);
        for (int i = 0; i<number; i++)
        {
            newRewards[i] = shuffleRewards[i];
           // Unregister(newRewards[i]);
        }
        return newRewards;
    }
}
public static class RewardFactory
{
    private static Dictionary<string, Type> rewardByName;
    private static bool isInit => rewardByName != null;
    private static void Init()
    {
        if (isInit) return;
        Debug.Log("初始化奖励脚本");

        var rewardTypes = Assembly.GetAssembly(typeof(IRewardable)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsInterface && myType.IsSubclassOf(typeof(Rewardable)));
        rewardByName = new Dictionary<string, Type>();
        foreach (var rewardType in rewardTypes)
        {
            var tempEffect = Activator.CreateInstance(rewardType, new Reward()) as IRewardable;
            Debug.Log(tempEffect.Name);
            rewardByName.Add(tempEffect.Name, rewardType);
        }
    }
    public static IRewardable GetReward(string rewardType, Reward reward)
    {
        Init();
        if (rewardByName.ContainsKey(rewardType))
        {
            Type type = rewardByName[rewardType];
            try
            {
                var rewardInstance = Activator.CreateInstance(type, reward) as IRewardable;
                return rewardInstance;
            }
            catch (Exception ex)
            {
                Debug.LogError($"无法创建奖励实例 {rewardType}: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"未找到奖励类型 {rewardType}");
        }
        return null;
    }
    public static IEnumerable<string> GetRewardNames()
    {
        Debug.Log("获取所有奖励名称");
        Init();
        return rewardByName.Keys;
    }
}
