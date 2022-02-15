using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefDataProvider : IDataProvider
{
    private const string DATA_PATH = "DataSave";

    public GameData LoadData()
    {
        GameData data;
        if (PlayerPrefs.HasKey(DATA_PATH))
        {
            var dataString = PlayerPrefs.GetString(DATA_PATH);
            data = Newtonsoft.Json.JsonConvert.DeserializeObject<GameData>(dataString);
        }
        else
            data = new GameData();
        return data;
    }

    public void SyncData(GameData gameData, bool isForced)
    {
        SaveData(gameData);
    }

    private void SaveData(GameData gameData)
    {
        var dataString = Newtonsoft.Json.JsonConvert.SerializeObject(gameData);
        PlayerPrefs.SetString(DATA_PATH, dataString);
        PlayerPrefs.Save();
    }
}
