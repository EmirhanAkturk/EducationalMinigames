using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "lib/Mat Collection", fileName = "ColorCollection")]
public class MatCollection : ScriptableObject
{
	[SerializeField] private List<GMat> mats = new List<GMat>();
	public List<GMat> Mats => mats;
}
