using System;
using System.Collections;
using System.Collections.Generic;
using lib.GameDepends.Enums;
using Minigames.SwordAndPistol.Scripts.UI_Scripts.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Minigames.SwordAndPistol.Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public static readonly UnityEvent<MiniGameType> OnGameStart = new UnityEvent<MiniGameType>();
        public static readonly UnityEvent<MiniGameType> OnGameOver = new UnityEvent<MiniGameType>();
        
        public const MiniGameType CURRENT_MINI_GAME_TYPE = MiniGameType.SwordAndPistol;

        
        public DifficultyType CurrentDifficultyType { get; private set; }
        public bool IsPlaying { get; private set; }
        
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private Image progressBarImage;
        [SerializeField] private GameObject timerUI_Gameobject;

        // [Header("Managers")]
        // [SerializeField] private GameObject cubeSpawnManager;

        private StartGamePanel StartGamePanel => startGamePanel ??= FindObjectOfType<StartGamePanel>(false);
        private StartGamePanel startGamePanel;

        private EndGamePanel EndGamePanel => endGamePanel ??= FindObjectOfType<EndGamePanel>(false);
        private EndGamePanel endGamePanel;
        
        private GameplayPanel GameplayPanel => gameplayPanel ??= FindObjectOfType<GameplayPanel>(false);
        private GameplayPanel gameplayPanel;

        private OVRCameraRig OvrCameraRig => ovrCameraRig ??= FindObjectOfType<OVRCameraRig>(false);
        private OVRCameraRig ovrCameraRig;

        //Audio related
        private float audioClipLength;
        private float timeToStartGame = 5.0f;


        #region Difficulty

        public void SetDifficulty(DifficultyType difficultyType)
        {
            CurrentDifficultyType = difficultyType;
        }

        public float GetCurrentSpawnPeriod()
        {
            return DifficultyService.GetDifficultySpawnPeriod(CurrentDifficultyType);
        }

        #endregion

        private void OnEnable()
        {
            MiniGameManager.Instance.ActiveMiniGameType = CURRENT_MINI_GAME_TYPE; // For test
            HideAllPanels();
            StartGamePanel.ShowPanel(GetPanelTargetPos());
            IsPlaying = false;
            OnGameStart.AddListener(StartGame);
        }
      
        private void OnDisable()
        {
            OnGameStart.RemoveListener(StartGame);
        }

        // Start is called before the first frame update
        private void StartGame(MiniGameType miniGameType)
        {
            if(miniGameType != CURRENT_MINI_GAME_TYPE) return;
            
            StartGamePanel.SetPanelState(false);

            //Getting the duration of the song
            audioClipLength = AudioManager.Instance.GetAudioSource(AudioType.MusicTheme).clip.length;
            AudioManager.Instance.PlaySound(AudioType.MusicTheme);
            Debug.Log(audioClipLength);

            //Starting the countdown with song
            StartCoroutine(StartCountdown(audioClipLength));

            //Resetting progress bar
            progressBarImage.fillAmount = 0;

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
            OnGameOver.Invoke(CURRENT_MINI_GAME_TYPE);
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
            IsPlaying = !isGameOver;
            // cubeSpawnManager.SetActive(!isGameOver);
            timerUI_Gameobject.SetActive(!isGameOver);
            GameplayPanel.SetPanelState(!isGameOver);
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
            return OvrCameraRig.transform.position + new Vector3(0, 2f, 4f);
        }
        
        private void HideAllPanels()
        {
            StartGamePanel.SetPanelState(false);
            EndGamePanel.SetPanelState(false);
            GameplayPanel.SetPanelState(false);
            timerUI_Gameobject.gameObject.SetActive(false);
        }
    }
}

public enum DifficultyType
{
    Easy = 0,
    Normal = 1,
    Hard = 2,
}

public class Difficulty
{
    public DifficultyType Type { get; private set; }
    public string Name{ get; private set; }
    public float CubeSpawnPeriod{ get; private set; }

    public Difficulty(DifficultyType type, string name, float cubeSpawnPeriod)
    {
        Type = type;
        Name = name;
        CubeSpawnPeriod = cubeSpawnPeriod;
    }
}
