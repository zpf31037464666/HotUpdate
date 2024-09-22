using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitData
{
    public bool isEnter=false;
}

public class FirstEnterGame : PersistentSingleton<FirstEnterGame>, ISaveable<InitData>
{
    public InitData initData;

    protected override void Awake()
    {
        base.Awake();
        RegisterSaveData();

        initData=new InitData();
        initData.isEnter=false;

    }
    private void Start()
    {
        LoadSaveData();

        if(!initData.isEnter)
        {
            Debug.LogWarning("第一次进游戏");
        }
    }
    public void EnentInitScenec()
    {
        initData.isEnter = true;
        SaveData();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("测试 第一次进入游戏");
            EnentInitScenec();
        }
    }

    #region saveData
    private void RegisterSaveData()
    {
        var playerSaveManager = SaveLoadManager<InitData>.GetInstance(GetType().Name);
        playerSaveManager.Register(this);
    }
    private void SaveData()
    {
        var playerSaveManager = SaveLoadManager<InitData>.GetInstance(GetType().Name);
        playerSaveManager.SaveGameData("Save1", GetType().Name);
    }
    private void LoadSaveData()
    {
        //加载场景
        var playerSaveManager = SaveLoadManager<InitData>.GetInstance(GetType().Name);
        playerSaveManager.LoadGameData("Save1", GetType().Name);
    }
    public InitData GenerateGameData()
    {
      return initData;
    }
    public string GetDataID()
    {
        return GetType().Name;
    }
    public void RestoreGameData(InitData data)
    {
        this.initData = data;
    }
    #endregion


}
