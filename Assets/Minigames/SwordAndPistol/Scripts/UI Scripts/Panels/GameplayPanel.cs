using Minigames.SwordAndPistol.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts.UI_Scripts.Panels
{
    public class GameplayPanel : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI currentScoreText;
     
        
        public void SetPanelState(bool isShow)
        {
            gameObject.SetActive(isShow);

            if (isShow)
            {
                OnUpdateScoreText(0);
            }
        }

        private void OnUpdateScoreText(int score = -1)
        {
            currentScoreText.text = score.ToString();
        }

        private void OnEnable()
        {
            ScoreManager.Instance.OnScoreChanged.AddListener(OnUpdateScoreText);
        }
        
        private void OnDisable()
        {
            if(ScoreManager.IsAvailable())
                ScoreManager.Instance.OnScoreChanged.RemoveListener(OnUpdateScoreText);
        }
    }
}
