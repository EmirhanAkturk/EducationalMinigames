using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCollection", menuName = "lib/LevelCollection")]
public class LevelCollection : ScriptableObject
{
    [SerializeField] private List<Level> Levels;

    //Key = LevelId, Value = Level Data
    private Dictionary<int, Level> levelsDictionary = new Dictionary<int, Level>();
    private Dictionary<int, Level> levelsDictionarybyOrder = new Dictionary<int, Level>();

    public Dictionary<int, Level> LevelsDictionary
    {
        get
        {
            if (!isLoaded) Load();

            return levelsDictionary;
        }
    }

    [NonSerialized] private bool isLoaded;

    private void Load()
    {
        foreach (var item in Levels)
        {
            levelsDictionary[item.LevelId] = item;
            levelsDictionarybyOrder[item.Order] = item;
        }
        isLoaded = true;
    }

    public Level GetLevelByOrder(int _order)
    {
        if (!isLoaded) Load();
        return levelsDictionarybyOrder[_order];
    }
    
    public int GetOrderById(int levelId)
    {
        if (!isLoaded) Load();
        return levelsDictionary[levelId].Order;
    }

    public bool IsLevelExist (int _order)
    {
        if (!isLoaded) Load();
        return levelsDictionarybyOrder.ContainsKey(_order);
    }
}

[Serializable]
public class Level
{
    public int LevelId;
    public int Order;
    public string SceneName;
}