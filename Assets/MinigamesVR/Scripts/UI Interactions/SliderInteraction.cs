using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGamesVR.Scripts.UI_Interactions
{
    public class SliderInteraction : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI sliderTextValue;
        [SerializeField] private Slider slider;

        private void Start()
        {
            sliderTextValue.text = "Slider Value : " + slider.value;
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            sliderTextValue.text = "Slider Value : " + slider.value;
        }
    }
}
