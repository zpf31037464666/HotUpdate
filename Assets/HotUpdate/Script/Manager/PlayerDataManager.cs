using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int coin;//金币
    public int gem;//宝石

   public PlayerData()
    {
        coin = 0;
        gem = 0;
    }
}


public class PlayerDataManager : PersistentSingleton<PlayerDataManager>, ISaveable<PlayerData>
{
    public PlayerData playerData=new PlayerData();

    private void Start()
    {
        RegisterSaveData();

        LoadSaveData();
    }

    public void AddCoin(int number)
    {
        playerData.coin+=number;
      //  MessageManager.instance.SendMessage("增加金币"+number.ToString());
        SaveData();
    }
    public void AddGem(int number)
    {
        playerData.gem+=number;
        SaveData();
    }
    public void RemoveCoin(int number)
    {
        playerData.coin-=number;
        SaveData();
    }
    public void RemoveGem(int number)
    {
        playerData.gem-=number;
        SaveData();
    }

    public int GetCoin() => playerData.coin;
    public int GetGem() => playerData.gem;
    public bool ComPareCoin(int number)
    {
        return playerData.coin>=number;
    }
    public bool ComPareGem(int number)
    {
        return playerData.gem>=number;
    }


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        Debug.Log("增加金币"+GetType().Name);

    //        AddCoin(1000);

    //    }
    //}


    private void RegisterSaveData()
    {
        var playerSaveManager = SaveLoadManager<PlayerData>.GetInstance(GetType().Name);
        playerSaveManager.Register(this);
    }
    private void SaveData()
    {
        var playerSaveManager = SaveLoadManager<PlayerData>.GetInstance(GetType().Name);
        playerSaveManager.SaveGameData("Save1", GetType().Name);
    }
    private void LoadSaveData()
    {
        //加载场景
        var playerSaveManager = SaveLoadManager<PlayerData>.GetInstance(GetType().Name);
        playerSaveManager.LoadGameData("Save1", GetType().Name);
    }
    public string GetDataID()
    {
        return GetType().Name;
    }

    public PlayerData GenerateGameData()
    {
        return playerData;
    }

    public void RestoreGameData(PlayerData data)
    {
        playerData = data;
    }
}
