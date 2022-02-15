using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CollectableObjectData
{
	public int Id;
	public int Cost;
	public CollectableObjectType CollectableObjectType;
	public PoolType FoldedType; //Katlanmış hali
	public PoolType HangedType; //Askıdaki hali
	public PoolType CarryType; //Taşınırkenki hali
	public CarryPointType CarryPointsType;
	//[SerializeField] List<IconBlock> iconList = new List<IconBlock>();
	public Sprite Icon;
	public Sprite BagIcon;

	public PoolType GetShowPoolType(CollectableShowType showType)
    {
        switch (showType)
        {
			case CollectableShowType.Folded:
				return FoldedType;
			case CollectableShowType.Hanged:
				return HangedType;
			default:
				return CarryType;
        }
    }

	[NonSerialized] private bool isLoaded = false;
	private Dictionary<MatColorGroup, Sprite> iconDictionary = new Dictionary<MatColorGroup, Sprite>();
	/*private Dictionary<MatColorGroup, Sprite> IconDictionary
    {
        get
        {
            if (!isLoaded)
            {
                foreach (var item in iconList)
                {
					iconDictionary[item.ColorCode] = item.Icon;
					bagIconDictionary[item.ColorCode] = item.BagIcon;
				}
				isLoaded = true;
            }
			return iconDictionary;
		}
    }*/

	private Dictionary<MatColorGroup, Sprite> bagIconDictionary = new Dictionary<MatColorGroup, Sprite>();
	/*private Dictionary<MatColorGroup, Sprite> BagIconDictionary
	{
		get
		{
			if (!isLoaded)
			{
				foreach (var item in iconList)
				{
					iconDictionary[item.ColorCode] = item.Icon;
					bagIconDictionary[item.ColorCode] = item.BagIcon;
				}
				isLoaded = true;
			}
			return bagIconDictionary;
		}
	}*/

	public Sprite GetIcon(MatColorGroup ColorCode)
    {
		return Icon;
		/*if (IconDictionary == null || IconDictionary.Count == 0)
			return null;

		if (IconDictionary.ContainsKey(ColorCode))
			return IconDictionary[ColorCode];
		return IconDictionary.First().Value;*/
	}

	public Sprite GetBagIcon(MatColorGroup ColorCode)
	{
		return BagIcon;
		/*if (BagIconDictionary == null || BagIconDictionary.Count == 0)
			return null;

		if (BagIconDictionary.ContainsKey(ColorCode))
			return BagIconDictionary[ColorCode];
		return BagIconDictionary.First().Value;*/
	}
}

[Serializable]
public class IconBlock
{
	public MatColorGroup ColorCode;
	public Sprite Icon;
	public Sprite BagIcon;
}

public enum CollectableShowType
{
	Folded = 1,
	Hanged = 2,
	Carry = 3
}