using System;
using Minigames.SwordAndPistol.Scripts.Managers;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using GameManager = Minigames.SwordAndPistol.Scripts.Managers.GameManager;

namespace Minigames.SwordAndPistol.Scripts.UI_Scripts.Panels
{
    public class EndGamePanel : MonoBehaviourPunCallbacks
    {
        [Header("Buttons")] 
        [SerializeField] private Button restartButton; 
        [SerializeField] private Button returnLobbyButton;

        [Header("Texts")] 
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TextMeshProUGUI finalScoreText;

        #region Photon Callbacks

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            MiniGameManager.Instance.LoadHomeScene();
        }

        #endregion
        
        public void SetPanelState(bool isShow)
        {
            gameObject.SetActive(isShow);
        }

        public void ShowPanel(Vector3 targetPos)
        {
            highScoreText.text = ScoreManager.Instance.HighScore.ToString();
            finalScoreText.text = ScoreManager.Instance.CurrentScore.ToString();
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.position = targetPos;
            SetPanelState(true);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            restartButton.onClick.AddListener(OnRestartGame);
            returnLobbyButton.onClick.AddListener(OnBackToLobby);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            restartButton.onClick.RemoveListener(OnRestartGame);
            returnLobbyButton.onClick.RemoveListener(OnBackToLobby);
        }

        private void OnRestartGame()
        {
            MiniGameManager.Instance.RestartMiniGame();
        }

        private void OnBackToLobby()
        {
            PhotonNetwork.LeaveRoom();
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
