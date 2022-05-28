using System;
using Minigames.SwordAndPistol.Scripts.Managers;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameManager = Minigames.SwordAndPistol.Scripts.Managers.GameManager;

namespace Minigames.SwordAndPistol.Scripts.UI_Scripts.Panels
{
    public class StartGamePanel : MonoBehaviour
    {
        
        [Header("Buttons")] 
        [SerializeField] private Button startButton; 
        [SerializeField] private Button returnLobbyButton;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI highScoreText;
        
        public void SetPanelState(bool isShow)
        {
            gameObject.SetActive(isShow);
        }

        public void ShowPanel(Vector3 targetPos)
        {
            highScoreText.text = ScoreManager.Instance.HighScore.ToString();
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.position = targetPos;
            SetPanelState(true);
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
            GameManager.OnGameStart.Invoke(MiniGameManager.Instance.ActiveMiniGameType);
        }

        private void OnBackToLobby()
        {
            
        }
    }
}
