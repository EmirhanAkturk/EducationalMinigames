using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGameCollection", menuName = "lib/MiniGameCollection")]
public class MiniGameCollection : ScriptableObject
{
    [SerializeField] private List<MiniGame> miniGames;

    private Dictionary<MiniGameType, MiniGame> miniGamesDictionary = new Dictionary<MiniGameType, MiniGame>();
    
    public Dictionary<MiniGameType, MiniGame> MiniGamesDictionary
    {
        get
        {
            if (!isLoaded) Load();

            return miniGamesDictionary;
        }
    }

    [NonSerialized] private bool isLoaded;

    private void Load()
    {
        foreach (var miniGame in miniGames)
        {
            miniGamesDictionary[miniGame.miniGameType] = miniGame;
        }
        isLoaded = true;
    }

    public MiniGame GetMiniGameByType(MiniGameType miniGameType)
    {
        return MiniGamesDictionary.ContainsKey(miniGameType) ? MiniGamesDictionary[miniGameType] : null;
    }
}

[Serializable]
public class MiniGame
{
    public MiniGameType miniGameType;
    public string SceneName;
}