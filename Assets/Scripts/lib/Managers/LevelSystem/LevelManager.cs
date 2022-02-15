using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    private LevelCollection LevelCollection;
    private const string COLLECTION_PATH = "Configurations/LevelCollection";
    private bool isLoaded = false;
    public  int ActiveLevelId;
    public Level MaxLevel;
    public UnityEvent LevelLoaded = new UnityEvent();
    public UnityEvent BeforeLevelLoad = new UnityEvent();

    private void Load()
    {
        if (isLoaded) return;
        LevelCollection = Resources.Load<LevelCollection>(COLLECTION_PATH);
        SceneManager.sceneLoaded += sceneChanged;
        LoadData();
    }

    void sceneChanged(Scene sc, LoadSceneMode sm)
    {
        LevelLoaded?.Invoke();
    }

    private void LoadData()
    {
        var levelData = DataService.GetData<Dictionary<StateType, int>>(DataType.STATE);
        
        if(levelData == null)
        {
            levelData = new Dictionary<StateType, int>();
        }
        if (levelData.ContainsKey(StateType.Level))
        {
            MaxLevel = LevelCollection.LevelsDictionary[levelData[StateType.Level]];
        }
        else
        {
            MaxLevel = LevelCollection.GetLevelByOrder(0);
        }

        if (levelData.ContainsKey(StateType.ActiveLevel))
        {
            ActiveLevelId = levelData[StateType.ActiveLevel];
        }
        else
        {
            ActiveLevelId = MaxLevel.LevelId;
        }

        Debug.Log("Level Loaded Active: " + ActiveLevelId + " max: " + MaxLevel.LevelId);
        isLoaded = true;
    }

    private void SaveData()
    {
        if (!isLoaded) return;
        var levelData = DataService.GetData<Dictionary<StateType, int>>(DataType.STATE);
        levelData[StateType.ActiveLevel] = ActiveLevelId;
        levelData[StateType.Level] = MaxLevel.LevelId;
        DataService.SetData(DataType.STATE, levelData);
    }

    public void LoadLevel(int _activeLevel = -1)
    {
        Load();
        var currentLevel = _activeLevel != -1 ? _activeLevel : ActiveLevelId;
        ActiveLevelId = currentLevel;
        if(MaxLevel.Order < LevelCollection.LevelsDictionary[ActiveLevelId].Order)
        {
            LevelUp();
        }
        BeforeLevelLoad?.Invoke();
        SceneManager.LoadScene(LevelCollection.LevelsDictionary[ActiveLevelId].SceneName);
        PanelManager.Instance.Show(PopupType.SwitchScenePanel, new PanelData());
        if(_activeLevel != -1) SaveData();
    }

    public bool LevelUp()
    {
        Load();
        if(LevelCollection.IsLevelExist(MaxLevel.Order + 1))
        {
            MaxLevel = LevelCollection.GetLevelByOrder(MaxLevel.Order + 1);
            SaveData();
            return true;
        }
        return false;
    }

    public void ResetLevel()
    {
        MaxLevel = LevelCollection.GetLevelByOrder(0);
    }

    public int GetActiveLevelOrder()
    {
        return LevelCollection.GetOrderById(ActiveLevelId);
    }    
    
    public int GetOrderByLevelId(int levelId)
    {
        return LevelCollection.GetOrderById(levelId);
    }
}
