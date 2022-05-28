using System;
using System.Collections;
using System.Collections.Generic;
using lib.GameDepends.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGameCollection", menuName = "lib/MiniGameCollection")]
public class MiniGameCollection : ScriptableObject
{
    [SerializeField] private List<MiniGame> miniGames;

    private Dictionary<MiniGameType, MiniGame> miniGamesDictionary = new Dictionary<MiniGameType, MiniGame>();

    private Dictionary<MiniGameType, MiniGame> MiniGamesDictionary
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
    [Tooltip("Part after Assets/Minigames ")] public string SceneFolderPath;
    public string SceneName;
}