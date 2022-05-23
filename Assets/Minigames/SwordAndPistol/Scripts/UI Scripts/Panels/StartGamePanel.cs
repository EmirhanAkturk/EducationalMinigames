using System;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.SwordAndPistol.Scripts.UI_Scripts.Panels
{
    public class StartGamePanel : MonoBehaviour
    {
        
        [Header("Buttons")] 
        [SerializeField] private Button restartButton; 
        [SerializeField] private Button returnLobbyButton; 
        
        public void SetPanelState(bool isShow)
        {
            gameObject.SetActive(isShow);
        }

        public void ShowPanel(Vector3 targetPos)
        {
            gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            gameObject.transform.position = targetPos;
        }

        
        private void OnEnable()
        {
            restartButton.onClick.AddListener(OnRestartGame);
            returnLobbyButton.onClick.AddListener(OnBackToLobby);
        }
        
        private void OnDisable()
        {
            restartButton.onClick.RemoveListener(OnRestartGame);
            returnLobbyButton.onClick.RemoveListener(OnBackToLobby);
        }

        private void OnRestartGame()
        {
            MiniGameManager.Instance.RestartMiniGame();
        }

        private void OnBackToLobby()
        {
            
        }

        // public void OnButton1Clicked()
        // {
        //     simpleUIText.text = "Button1 is clicked";
        // }
        //
        // public void OnButton2Clicked()
        // {
        //     simpleUIText.text = "Button2 is clicked";
        // }
        //
        //
        // public void OnButton3Clicked()
        // {
        //     simpleUIText.text = "Button3 is clicked";
        // }
    }
}
