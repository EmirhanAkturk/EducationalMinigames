using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "lib/Audio Collection", fileName = "AudioCollection")]
public class AudioCollection : ScriptableObject
{
	[SerializeField] private List<GAudio> audio;
	public List<GAudio> Audio => audio;
}