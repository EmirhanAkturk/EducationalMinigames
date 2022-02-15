using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class MuteAudioToggle : MonoBehaviour
{

	[SerializeField] private float transitionTime = 0.5f;
	[SerializeField] private Image onImage;
	[SerializeField] private Image offImage;
	private bool isOn;

	private void Awake()
	{
		isOn = AudioService.Muted == 0 ? true : false; //Load
		GetComponent<Button>().onClick.AddListener(OnButtonClick);
		UpdateUI(0f);
	}

	private void OnButtonClick()
	{
		isOn = !isOn;
		if (isOn) AudioService.UnMuteAll();
		else AudioService.MuteAll();
		UpdateUI(transitionTime);
	}
	private void UpdateUI(float tTime)
	{
		DOTween.Kill(onImage);
		DOTween.Kill(offImage);
		if (isOn)
		{
			onImage.DOFade(1, tTime);
			offImage.DOFade(0, tTime);
		}
		else
		{
			onImage.DOFade(0, tTime);
			offImage.DOFade(1, tTime);
		}
	}
}
