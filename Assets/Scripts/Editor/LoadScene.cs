using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

public class LoadScene : Editor
{
    [MenuItem("LoadScene/Init", priority = 0)]
    private static void Init()
    {
        EditorSceneManager.OpenScene("Assets/Minigames/SwordAndPistol/Scenes/LobbyScene.unity");
        // EditorSceneManager.OpenScene("Assets/Scenes/InitScene.unity");
    }

    [MenuItem("LoadScene/MiniGames/SwordAndPistol", priority = 1)]
    private static void SwordAndPistol()
    {
        LoadMiniGame(MiniGameType.SwordAndPistol);
    }

    private static void LoadMiniGame(MiniGameType miniGameType)
    {
        var miniGameCollection = Resources.Load<MiniGameCollection>("Configurations/MiniGameCollection");
        var miniGame = miniGameCollection.GetMiniGameByType(miniGameType);
        if (miniGame == null) return;
        
        var sceneFolderPath = miniGame.SceneFolderPath;
        var sceneName = miniGame.SceneName;
        
        EditorSceneManager.OpenScene("Assets/MiniGames/" + sceneFolderPath + sceneName + ".unity");
    }

}
#endif