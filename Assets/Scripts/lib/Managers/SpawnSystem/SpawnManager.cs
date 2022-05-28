using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using lib.GameDepends.Enums;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : Singleton<SpawnManager>
{
    private readonly Dictionary<MiniGameType, Dictionary<SpawnPointType, List<ISpawnPoint>>> spawnPointsDictionary = new Dictionary<MiniGameType, Dictionary<SpawnPointType, List<ISpawnPoint>>>();
    
    #region Init

    void OnEnable ()
    {
        MiniGameManager.Instance.MiniGameLoaded.AddListener(SpawnAllSpawnPoints);
    }

    private void OnDisable()
    {
        if (MiniGameManager.IsAvailable())
        {
            MiniGameManager.Instance.MiniGameLoaded.RemoveListener(SpawnAllSpawnPoints);
        }
    }

    private void SpawnAllSpawnPoints(MiniGameType miniGameType)
    {
        if (!spawnPointsDictionary.ContainsKey(miniGameType) || spawnPointsDictionary[miniGameType].Count == 0)
        {
            Debug.Log($"{spawnPointsDictionary.ContainsKey(miniGameType)}, {spawnPointsDictionary[miniGameType].Count == 0}");
            Debug.Log($"There is no spawn point for {miniGameType}!!!");
            return;
        }

        var miniGameAllSpawnPoints = GetSpawnPointsByMiniGameType(miniGameType).Values;
        foreach (var spawnPoint in miniGameAllSpawnPoints.SelectMany(spawnPointsList => spawnPointsList))
        {
            spawnPoint.Spawn();
        }
    }

    #endregion

    public void AddSpawnPoint(MiniGameType miniGameType, SpawnPointType spawnPointType, ISpawnPoint spawnPoint)
    {
        if(!spawnPointsDictionary.ContainsKey(miniGameType))
            spawnPointsDictionary.Add(miniGameType, new Dictionary<SpawnPointType, List<ISpawnPoint>>());
        
        if(!spawnPointsDictionary[miniGameType].ContainsKey(spawnPointType))
            spawnPointsDictionary[miniGameType].Add(spawnPointType, new List<ISpawnPoint>());
        
        spawnPointsDictionary[miniGameType][spawnPointType].Add(spawnPoint);
    }    
    
    public void RemoveSpawnPoint(MiniGameType miniGameType, SpawnPointType spawnPointType, ISpawnPoint spawnPoint)
    {
        if (!spawnPointsDictionary.ContainsKey(miniGameType) ||
            !spawnPointsDictionary[miniGameType].ContainsKey(spawnPointType))
        {
            var isMiniGameExist = spawnPointsDictionary.ContainsKey(miniGameType);
            Debug.Log($"Spawn point couldn't remove!! \n " +
                      $"{(!isMiniGameExist ? $"{miniGameType} doesn't exits!!" : $"{spawnPointType} doesn't exits for {miniGameType}!!" )}");
            return;
        }
        spawnPointsDictionary[miniGameType][spawnPointType].Remove(spawnPoint);
    }
    
    public Dictionary<SpawnPointType, List<ISpawnPoint>> GetSpawnPointsByMiniGameType(MiniGameType miniGameType)
    {
        return spawnPointsDictionary.ContainsKey(miniGameType) ? spawnPointsDictionary[miniGameType] : new Dictionary<SpawnPointType, List<ISpawnPoint>>();
    }        
    
    public List<ISpawnPoint> GetSpawnPointsByType(MiniGameType miniGameType, SpawnPointType spawnPointType)
    {
        var miniGameSpawnPoints = GetSpawnPointsByMiniGameType(miniGameType);
        return miniGameSpawnPoints.ContainsKey(spawnPointType) ? miniGameSpawnPoints[spawnPointType] : new List<ISpawnPoint>();
    }    
    
    public ISpawnPoint GetFirstSpawnPointByType(MiniGameType miniGameType, SpawnPointType spawnPointType)
    {
        var spawnPoints = GetSpawnPointsByType(miniGameType, spawnPointType); 
        return spawnPoints.Count > 0 ? spawnPoints[0] : null;
    }
}

public enum SpawnPointType
{
    MainCharacterSpawnPoint = 0,
}
