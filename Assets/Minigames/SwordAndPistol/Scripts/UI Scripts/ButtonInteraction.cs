using TMPro;
using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts.UI_Scripts
{
    public class ButtonInteraction : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI simpleUIText;

        public void OnButton1Clicked()
        {
            simpleUIText.text = "Button1 is clicked";
        }

        public void OnButton2Clicked()
        {
            simpleUIText.text = "Button2 is clicked";
        }


        public void OnButton3Clicked()
        {
            simpleUIText.text = "Button3 is clicked";
        }


    }
}
