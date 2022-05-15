using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    private void Start()
    {
        MiniGameManager.Instance.LoadMiniGame(MiniGameType.SwordAndPistol);
    }
}
