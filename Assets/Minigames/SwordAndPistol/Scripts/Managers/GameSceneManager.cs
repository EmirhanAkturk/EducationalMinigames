using System;
using System.Collections;
using Minigames.SwordAndPistol.Scripts.UI_Scripts.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.SwordAndPistol.Scripts
{
    public class GameSceneManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private Image progressBarImage;
        [SerializeField] private GameObject timerUI_Gameobject;

        [Header("Managers")]
        [SerializeField] private GameObject cubeSpawnManager;

        [Header("UI Panels")]
        [SerializeField] private GameObject currentScoreUI;
        
        private StartGamePanel StartGamePanel => startGamePanel ??= FindObjectOfType<StartGamePanel>();
        private StartGamePanel startGamePanel;

        private EndGamePanel EndGamePanel => endGamePanel ??= FindObjectOfType<EndGamePanel>();
        private EndGamePanel endGamePanel;

        private OVRCameraRig OvrCameraRig => ovrCameraRig ??= FindObjectOfType<OVRCameraRig>();
        private OVRCameraRig ovrCameraRig;
        
        //Audio related
        private float audioClipLength;
        private float timeToStartGame = 5.0f;

        // private void Awake()
        // {
        //     SetGameUIState(false);
        //     StartGamePanel.ShowPanel(GetPanelTargetPos());
        // }

        private void Start()
        {
            StartGame();
        }

        // Start is called before the first frame update
        private void StartGame()
        {
            //Getting the duration of the song
            audioClipLength = AudioManager.Instance.GetAudioSource(AudioType.MusicTheme).clip.length;
            Debug.Log(audioClipLength);

            //Starting the countdown with song
            StartCoroutine(StartCountdown(audioClipLength));

            //Resetting progress bar
            progressBarImage.fillAmount = Mathf.Clamp(0, 0, 1);

            SetGameUIState(false);
        }

        private IEnumerator StartCountdown(float countdownValue)
        {
            while (countdownValue > 0)
            {
                yield return new WaitForSeconds(1.0f);
                countdownValue -= 1;

                timeText.text = ConvertToMinAndSeconds(countdownValue);

                progressBarImage.fillAmount = (AudioManager.Instance.GetAudioSource(AudioType.MusicTheme).time / audioClipLength);

            }
            GameOver();
        }

        private void GameOver()
        {
            timeText.text = ConvertToMinAndSeconds(0);
            SetGameUIState(true);
        }

        // private void SetGameUIState(bool isGameOver)
        // {
        //     cubeSpawnManager.SetActive(!isGameOver);
        //     timerUI_Gameobject.SetActive(!isGameOver);
        //     currentScoreUI.SetActive(!isGameOver);
        //
        //     EndGamePanel.SetPanelState(isGameOver);
        //
        //     if (isGameOver)
        //     {
        //         EndGamePanel.ShowPanel(GetPanelTargetPos());
        //     }
        //
        // }
        //
        private void SetGameUIState(bool isGameOver)
        {
            cubeSpawnManager.SetActive(!isGameOver);
            timerUI_Gameobject.SetActive(!isGameOver);
            currentScoreUI.SetActive(!isGameOver);

            EndGamePanel.SetPanelState(isGameOver);

            if (isGameOver)
            {
                EndGamePanel.ShowPanel(GetPanelTargetPos());
            }
        
        }

        private string ConvertToMinAndSeconds(float totalTimeInSeconds)
        {
            string timeText = Mathf.Floor(totalTimeInSeconds / 60).ToString("00") + ":" + Mathf.FloorToInt(totalTimeInSeconds % 60).ToString("00");
            return timeText;
        }

        private Vector3 GetPanelTargetPos()
        {
            var targetPos = OvrCameraRig.transform.position + new Vector3(0, 2f, 4f);
            return targetPos;
        }
    }
}
