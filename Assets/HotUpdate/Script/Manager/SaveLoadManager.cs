using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveLoadManager<T> where T : class
{
    private string jsonFolder; // 文件路径
    private HashSet<ISaveable<T>> saveableList = new HashSet<ISaveable<T>>();
    private Dictionary<string, Dictionary<string, T>> saveDataDict = new Dictionary<string, Dictionary<string, T>>();

    private static Dictionary<string, SaveLoadManager<T>> instances = new Dictionary<string, SaveLoadManager<T>>();

    // 构造函数，接受类型标识符
    private SaveLoadManager(string typeIdentifier)
    {
    }

    // 单例获取方法
    public static SaveLoadManager<T> GetInstance(string typeIdentifier)
    {
        if (!instances.ContainsKey(typeIdentifier))
        {
            instances[typeIdentifier] = new SaveLoadManager<T>(typeIdentifier);
        }
        return instances[typeIdentifier];
    }

    // 注册方法
    public void Register(ISaveable<T> saveable)
    {
        if (!saveableList.Contains(saveable))
        {
            Debug.Log(saveable.GetType().Name + " 注册了事件");
            saveableList.Add(saveable);
        }
    }

    // 注销方法
    public void Unregister(ISaveable<T> saveable)
    {
        if (saveableList.Contains(saveable))
        {
            Debug.Log(saveable.GetType().Name + " 注销了事件");
            saveableList.Remove(saveable);
        }
    }

    /// <summary>
    /// 保存单一数据  list<T>  typeof(T).name 无论什么都为 list1 会造成数据覆盖
    /// </summary>
    /// <param name="saveIdentifier"></param>
    public void SaveGameData(string saveIdentifier)
    {
        var saveDataDic = new Dictionary<string, T>();
        foreach (var saveable in saveableList)
        {
            saveDataDic.Add(saveable.GetDataID(), saveable.GenerateGameData());
        }
        saveDataDict[saveIdentifier] = saveDataDic;

        // 使用存档标识符和类型标识符创建文件路径
        var resultPath = Application.persistentDataPath + "/Save/" + saveIdentifier + "/" + typeof(T).Name + ".json";
        Debug.Log("保存数据 " + resultPath);
        var jsonData = JsonConvert.SerializeObject(saveDataDic, Formatting.Indented);

        // 创建存档文件夹
        var folderPath = Path.GetDirectoryName(resultPath);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        File.WriteAllText(resultPath, jsonData);
    }
    /// <summary>
    /// 存储  list<T>  自己设名字
    /// </summary>
    /// <param name="saveIdentifier"></param>
    public void SaveGameData(string saveIdentifier, string jsonName)
    {
        var saveDataDic = new Dictionary<string, T>();
        foreach (var saveable in saveableList)
        {
            //saveDataDic.Add(saveable.GetDataID(), saveable.GenerateGameData());
            var dataID = saveable.GetDataID();
            if (!saveDataDic.ContainsKey(dataID))
            {
                saveDataDic.Add(dataID, saveable.GenerateGameData());
            }
            else
            {
                Debug.LogWarning($"Key {dataID} already exists in saveDataDic. Consider updating its value.");
                saveDataDic[dataID] = saveable.GenerateGameData(); // 或者选择更新值
            }
        }
        saveDataDict[saveIdentifier] = saveDataDic;

        // 使用存档标识符和类型标识符创建文件路径
        var resultPath = Application.persistentDataPath + "/Save/" + saveIdentifier + "/" + jsonName+ ".json";
        Debug.Log("保存数据 " + resultPath);
        var jsonData = JsonConvert.SerializeObject(saveDataDic, Formatting.Indented);

        // 创建存档文件夹
        var folderPath = Path.GetDirectoryName(resultPath);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        File.WriteAllText(resultPath, jsonData);
    }

    /// <summary>
    /// 读取单一数据
    /// </summary>
    /// <param name="saveIdentifier"></param>
    // 加载数据
    public void LoadGameData(string saveIdentifier)
    {
        var resultPath = Application.persistentDataPath + "/Save/" + saveIdentifier + "/" + typeof(T).Name + ".json";
        Debug.Log("加载数据 " + resultPath);
        if (!File.Exists(resultPath)) return;

        var stringData = File.ReadAllText(resultPath);
        var jsonData = JsonConvert.DeserializeObject<Dictionary<string, T>>(stringData);

        saveDataDict[saveIdentifier] = jsonData;

        foreach (var saveable in saveableList)
        {
            if (jsonData.ContainsKey(saveable.GetDataID()))
            {
                saveable.RestoreGameData(jsonData[saveable.GetDataID()]);
            }
        }
    }
    /// <summary>
    ///  读取文件数据
    /// </summary>
    /// <param name="saveIdentifier">文件夹名，存档名</param>
    /// <param name="jsonName">数据名</param>
    public void LoadGameData(string saveIdentifier, string jsonName)
    {
        var resultPath = Application.persistentDataPath + "/Save/" + saveIdentifier + "/" + jsonName + ".json";
        Debug.Log("加载数据 " + resultPath);
        if (!File.Exists(resultPath)) return;

        var stringData = File.ReadAllText(resultPath);
        var jsonData = JsonConvert.DeserializeObject<Dictionary<string, T>>(stringData);

        saveDataDict[saveIdentifier] = jsonData;

        foreach (var saveable in saveableList)
        {
            if (jsonData.ContainsKey(saveable.GetDataID()))
            {
                saveable.RestoreGameData(jsonData[saveable.GetDataID()]);
            }
        }
    }
    // 删除数据
    public void DeleteGameData(string saveIdentifier)
    {
        var resultPath = Application.persistentDataPath + "/Save/" + saveIdentifier + "/" + typeof(T).Name + ".json";
        if (File.Exists(resultPath))
        {
            File.Delete(resultPath);
        }

        saveDataDict.Remove(saveIdentifier);
    }

    // 获得数据
    public Dictionary<string, T> GetGameData(string saveIdentifier)
    {
        if (saveDataDict.TryGetValue(saveIdentifier, out var saveData))
        {
            return saveData;
        }
        else
        {
            var resultPath = Application.persistentDataPath + "/Save/" + saveIdentifier + "/" + typeof(T).Name + ".json";
            if (File.Exists(resultPath))
            {
                var stringData = File.ReadAllText(resultPath);
                var jsonData = JsonConvert.DeserializeObject<Dictionary<string, T>>(stringData);
                saveDataDict[saveIdentifier] = jsonData;
                return jsonData;
            }
        }

        return null;
    }

    // 获取具体值
    public T GetGameData(string saveIdentifier, string dataID)
    {
        var saveData = GetGameData(saveIdentifier);
        if (saveData != null && saveData.TryGetValue(dataID, out var gameData))
        {
            return gameData;
        }
        return null;
    }

    // 是否有存档
    public bool IsHaveSaveData(string saveIdentifier)
    {
        var resultPath = Application.persistentDataPath + "/Save/" + saveIdentifier + "/" + typeof(T).Name + ".json";
        return File.Exists(resultPath);
    }

    public void SetGameData(string saveIdentifier, string dataID, T gameData)
    {
        if (!saveDataDict.TryGetValue(saveIdentifier, out var saveData))
        {
            saveData = new Dictionary<string, T>();
            saveDataDict[saveIdentifier] = saveData;
        }
        saveData[dataID] = gameData;
    }
}
