using System;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Minigames.SwordAndPistol.Scripts.UI_Scripts.Panels
{
    public class StartGamePanel : MonoBehaviour
    {
        
        [Header("Buttons")] 
        [SerializeField] private Button startButton; 
        [SerializeField] private Button returnLobbyButton; 
        
        public void SetPanelState(bool isShow)
        {
            gameObject.SetActive(isShow);
        }

        public void ShowPanel(Vector3 targetPos)
        {
            SetPanelState(true);
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.position = targetPos;
        }

        private void OnEnable()
        {
            startButton.onClick.AddListener(OnStartGame);
            returnLobbyButton.onClick.AddListener(OnBackToLobby);
        }
        
        private void OnDisable()
        {
            startButton.onClick.RemoveListener(OnStartGame);
            returnLobbyButton.onClick.RemoveListener(OnBackToLobby);
        }

        private void OnStartGame()
        {
            EventService.OnGameStart.Invoke(MiniGameManager.Instance.ActiveMiniGameType);
        }

        private void OnBackToLobby()
        {
            
        }
    }
}
