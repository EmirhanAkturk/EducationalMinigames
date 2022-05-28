using System.Collections.Generic;
using Minigames.SwordAndPistol.Scripts.Managers;
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
			UpdateSliderText(slider.value);

			//Adds a listener to the main slider and invokes a method when the value changes.
			slider.onValueChanged.AddListener(OnSliderValueChanged);
		}

		private void OnSliderValueChanged(float value)
		{
			UpdateSliderText(value);
		}

		private void UpdateSliderText(float value)
		{
			Debug.Log("Value : " + value);
			var intValue = (int) value;

			string difficulty = DifficultyService.GetDifficultyName(intValue);
			sliderValueText.text = difficulty;
			
			var difficultyType = DifficultyService.GetDifficultyType(intValue);
			GameManager.Instance.SetDifficulty(difficultyType);
		}
	}
}

