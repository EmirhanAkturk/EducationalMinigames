using TMPro;
using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public int currentScore; 
        public int highScore;

        [Header("UI Fields")]
        [SerializeField] private  TextMeshProUGUI hightScoreText;
        [SerializeField] private  TextMeshProUGUI currentScoreText;
        [SerializeField] private  TextMeshProUGUI finalScoreText;

        // Start is called before the first frame update
        private void Start()
        {
            InitializeTexts();
        }

        private void InitializeTexts()
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
            hightScoreText.text = highScore.ToString();

            //Set the current score as 0.
            currentScoreText.text = "0";
        }


        public void AddScore(int scorePoint)
        {
            currentScore = currentScore + scorePoint;
            PlayerPrefs.SetInt("CurrentScore",currentScore);

            //Display the current score in UI
            currentScoreText.text = currentScore.ToString();

            //Also, update the final score
            finalScoreText.text = currentScore.ToString();

            if (currentScore > PlayerPrefs.GetInt("HighScore",0))
            {
                PlayerPrefs.SetInt("HighScore",currentScore);
                hightScoreText.text = currentScore.ToString();

            }
        }
    }
}
