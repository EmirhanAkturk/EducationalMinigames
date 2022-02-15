using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CollectableObjectService
{
	private static CollectableObjectCollection collectableObjectCollection;

	private static Dictionary<int, CollectableObjectData> collectableObjectDicById = new Dictionary<int, CollectableObjectData>();

	private const string COLLECTION_PATH = "Configurations/CollectableObjectCollection";

	static CollectableObjectService()
	{
		Load();
	}

	private static void Load()
	{
		collectableObjectCollection = Resources.Load<CollectableObjectCollection>(COLLECTION_PATH);

		foreach (var item in collectableObjectCollection.CollectableObjects)
			collectableObjectDicById[item.Id] = item;
	}

	public static CollectableObjectData GetCollectableObjectData(int itemId)
	{
		if (collectableObjectDicById.ContainsKey(itemId))
			return collectableObjectDicById[itemId];
		return null;
	}

	public static List<CollectableObjectData> GetCollectableObjectData(CollectableObjectType collectableObjectType)
	{
		return collectableObjectCollection.CollectableObjects.FindAll(item => item.CollectableObjectType == collectableObjectType);
	}
}
