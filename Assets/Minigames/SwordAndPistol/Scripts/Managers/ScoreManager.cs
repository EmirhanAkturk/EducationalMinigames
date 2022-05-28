using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Minigames.SwordAndPistol.Scripts.Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public UnityEvent<int> OnScoreChanged = new UnityEvent<int>();
        
        public int HighScore
        {
            get => PlayerPrefs.GetInt("HighScore", 0);
            private set => PlayerPrefs.SetInt("HighScore", value);
        }
        
        public int CurrentScore
        {
            get => PlayerPrefs.GetInt("CurrentScore", 0);
            private set => PlayerPrefs.SetInt("CurrentScore", value);
        }
        
        [Header("UI Fields")]
        [SerializeField] private TextMeshProUGUI currentScoreText;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        
        private int currentScore; 

        // Start is called before the first frame update
        private void Start()
        {
            InitializeTexts();
        }

        private void InitializeTexts()
        {
            //Set the current score as 0.
            currentScoreText.text = "0";
        }

        public void AddScore(int scorePoint)
        {
            currentScore += scorePoint;

            CurrentScore = currentScore;
            
            //Display the current score in UI
            currentScoreText.text = currentScore.ToString();

            //Also, update the final score
            finalScoreText.text = currentScore.ToString();

            if (currentScore > HighScore)
            {
                HighScore = currentScore;
            }
            
            OnScoreChanged.Invoke(currentScore);
        }
    }
}
