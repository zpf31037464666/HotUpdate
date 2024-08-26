using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class RewardManager :PersistentSingleton<RewardManager>
{
    private List<Reward> rewardDataList = new List<Reward>();
    private List<Rewardable> rewardList = new List<Rewardable>();

    private List<Rewardable> owerRawardList = new List<Rewardable>();

    void Start()
    {
        LoadJson();
        foreach (string name in RewardFactory.GetRewardNames())
        {
           // Debug.Log("奖励名字"+   name);
        }
    }
    private void LoadJson()
    {
        // 使用 Addressables 异步加载 JSON 文件
        Addressables.LoadAssetAsync<TextAsset>("Data/Reward.json").Completed += OnJsonLoaded;
    }
    private void OnJsonLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // JSON 文件加载成功
            TextAsset jsonAsset = handle.Result;
            string json = jsonAsset.text;
            rewardDataList = JsonConvert.DeserializeObject<List<Reward>>(json);
            foreach (var reward in rewardDataList)
            {
                GrantReward(reward.RewardID);
            }
            // 在这里处理 JSON 数据
            //暂时获得全部奖励 
            owerRawardList=rewardList;
            //每次被重新生成了
            Debug.Log("重新刷新奖励-------------------------");

        }
        else
        {
            Debug.LogError("Failed to load JSON: " + handle);
        }
    }

    private void GrantReward(int rewardID)
    {
        var data = rewardDataList.Find(r => r.RewardID == rewardID);
        Rewardable rewardable = RewardFactory.GetReward(data.RewardType, data);
        if (!rewardList.Contains(rewardable))
        {
            rewardList.Add(rewardable);
        }
    }

    public Rewardable[] GetOwerRewardable(int number)
    {
        Rewardable[] newRewards=new Rewardable[number];
        HashSet<int> rewardIdList = new HashSet<int>();
        int i = 0;
        while (i<number)
        {
            float randomValue =UnityEngine.Random.Range(0f, 1f);
            Rewardable reward = owerRawardList[UnityEngine.Random.Range(0, owerRawardList.Count)];
     
            if (reward.Reward.CumulativeProbability>randomValue&&reward.CanDraw())
            {
                if (!rewardIdList.Contains(reward.Reward.RewardID))
                {
                    rewardIdList.Add(reward.Reward.RewardID);
                    newRewards[i] = reward;
                    i++;
                }

            }
        }

        //for (int i = number - 1; i >= 0; i--)
        //{
        //    Rewardable reward = owerRawardList[UnityEngine.Random.Range(0, owerRawardList.Count)];
        //    newRewards[i] = reward;
        //}
        return newRewards;
    }

    //public IRewardable[] GetRewardble(int number)
    //{
    //    IRewardable[] newRewards = new IRewardable[number];
    //    // 存储已抽到的物品
    //    HashSet<int> rewardIdList = new HashSet<int>();
    //    int i = 0;
    //    while (i<number)
    //    {
    //        float randomValue =UnityEngine.Random.Range(0f, 1f);
    //        IRewardable reward= rewardables[UnityEngine.Random.Range(0, rewardables.Count)];
    //        if (reward.Reward.CumulativeProbability>randomValue)
    //        {
    //            if(!rewardIdList.Contains(reward.Reward.RewardID))
    //            {
    //                rewardIdList.Add(reward.Reward.RewardID);
    //                reward.Reward.currentCount++;
    //                newRewards[i] = reward;
    //                i++;
    //            }
    //        }
    //    }

    //    return newRewards;
    //}
}



public static class RewardFactory
{
    private static Dictionary<string, Type> rewardByName;
    private static bool isInit => rewardByName != null;
    private static void Init()
    {
        if (isInit) return;
       // Debug.Log("初始化奖励脚本");

        var rewardTypes = Assembly.GetAssembly(typeof(IRewardable)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsInterface && myType.IsSubclassOf(typeof(Rewardable)));
        rewardByName = new Dictionary<string, Type>();
        foreach (var rewardType in rewardTypes)
        {
            var tempEffect = Activator.CreateInstance(rewardType, new Reward()) as IRewardable;
            rewardByName.Add(tempEffect.Name, rewardType);
        }
    }
    public static Rewardable GetReward(string rewardType, Reward reward)
    {
        Init();
        if (rewardByName.ContainsKey(rewardType))
        {
            Type type = rewardByName[rewardType];
            try
            {
                var rewardInstance = Activator.CreateInstance(type, reward) as Rewardable;
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
       // Debug.Log("获取所有奖励名称");
        Init();
        return rewardByName.Keys;
    }
}
