using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.SwordAndPistol.Scripts.UI_Scripts
{
	public class SliderInteraction : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI sliderValueText;
		[SerializeField] private Slider slider;

		public void Start()
		{
			sliderValueText.text = slider.value.ToString();

			//Adds a listener to the main slider and invokes a method when the value changes.
			slider.onValueChanged.AddListener(OnSliderValueChanged);
		}

		public void OnSliderValueChanged(float value)
		{
			sliderValueText.text = "Slider Value: " + value;
		}
	}
}
