using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Only editable from Assets/Resource/COLLECTION_NAME
[Serializable]
public sealed class GMat
{
	[SerializeField] private MatColorGroup matColorGroup;
	[SerializeField] private MatType matType;

	[SerializeField] private Color color = Color.black;
	[SerializeField] private Texture2D texture;

	public MatType MatType => matType;
	public MatColorGroup MatColorGroup => matColorGroup;
	
	public Color Color => color;
	public Texture2D Texture => texture;

	public Material Material;

	private GMat() { } // Prevent Runtime creation
}