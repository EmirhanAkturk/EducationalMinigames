using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MiniGameManager : Singleton<MiniGameManager>
{
    private MiniGameCollection MiniGameCollection;
    private const string COLLECTION_PATH = "Configurations/MiniGameCollection";
    private bool isLoaded = false;
    public MiniGameType ActiveMiniGameType;
    public UnityEvent<MiniGameType> MiniGameLoaded = new UnityEvent<MiniGameType>();
    public UnityEvent<MiniGameType> BeforeMiniGameLoad = new UnityEvent<MiniGameType>();

    private void Load()
    {
        if (isLoaded) return;
        MiniGameCollection = Resources.Load<MiniGameCollection>(COLLECTION_PATH);
        SceneManager.sceneLoaded += SceneChanged;
        LoadData();
    }

    private void SceneChanged(Scene sc, LoadSceneMode sm)
    {
        MiniGameLoaded?.Invoke(ActiveMiniGameType);
    }

    private void LoadData()
    {
        var levelData = DataService.GetData<Dictionary<StateType, int>>(DataType.STATE);
        
        if(levelData == null)
        {
            levelData = new Dictionary<StateType, int>();
        }

        if (levelData.ContainsKey(StateType.ActiveLevel))
        {
            ActiveMiniGameType = (MiniGameType) levelData[StateType.ActiveLevel];
        }
        else
        {
            ActiveMiniGameType = MiniGameType.SwordAndPistol;
        }

        // Debug.Log("Level Loaded Active: " + ActiveLevelId + " max: " + MaxLevel.LevelId);
        isLoaded = true;
    }

    private void SaveData()
    {
        if (!isLoaded) return;
        var levelData = DataService.GetData<Dictionary<StateType, int>>(DataType.STATE);
        levelData[StateType.ActiveLevel] = (int)ActiveMiniGameType;
        // levelData[StateType.Level] = MaxLevel.LevelId;
        DataService.SetData(DataType.STATE, levelData);
    }
    
    public void LoadMiniGame(MiniGameType miniGameType)
    {
        Load();
        ActiveMiniGameType = miniGameType;

        // if(MaxLevel.Order < LevelCollection.LevelsDictionary[ActiveLevelId].Order)
        // {
        //     LevelUp();
        // }
        BeforeMiniGameLoad?.Invoke(miniGameType);
        SceneManager.LoadScene(GetMiniGameByType(ActiveMiniGameType).SceneName);
        // PanelManager.Instance.Show(PopupType.SwitchScenePanel, new PanelData());
        SaveData();
    }

    // public bool LevelUp()
    // {
    //     Load();
    //     if(LevelCollection.IsLevelExist(MaxLevel.Order + 1))
    //     {
    //         MaxLevel = LevelCollection.GetLevelByOrder(MaxLevel.Order + 1);
    //         SaveData();
    //         return true;
    //     }
    //     return false;
    // }

    // public void ResetLevel()
    // {
    //     MaxLevel = LevelCollection.GetLevelByOrder(0);
    // }

    private MiniGame GetMiniGameByType(MiniGameType miniGameType)
    {
        return MiniGameCollection.GetMiniGameByType(miniGameType);
    }
}
