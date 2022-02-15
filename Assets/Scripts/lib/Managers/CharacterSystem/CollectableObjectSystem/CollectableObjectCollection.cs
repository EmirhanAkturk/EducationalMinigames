using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectableObjectCollection", menuName = "lib/CollectableObjectCollection")]
public class CollectableObjectCollection : ScriptableObject
{
	public List<CollectableObjectData> CollectableObjects;
}