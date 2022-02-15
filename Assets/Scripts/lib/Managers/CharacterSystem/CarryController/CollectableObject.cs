using UnityEngine;

public class CollectableObject
{
    public int CollectableObjectId;
    public GMat ObjectMat { get; set; }
    [JsonIgnore] public GameObject ActiveGameObject;

    //Manifect eden yerden ilklendirilecek
    public CollectableObject(int collectableObjectId, GMat _objectMat)
    {
        CollectableObjectId = collectableObjectId;
        ObjectMat = _objectMat;
    }
    
    public CollectableObject(int collectableObjectId, GMat _objectMat, GameObject activeGameObject)
    {
        CollectableObjectId = collectableObjectId;
        ObjectMat = _objectMat;
        ActiveGameObject = activeGameObject;
    }
    
    public CollectableObject(CollectableObject otherCollectableObject)
    {
        CollectableObjectId = otherCollectableObject.CollectableObjectId;
        ObjectMat = otherCollectableObject.ObjectMat;
        ActiveGameObject = otherCollectableObject.ActiveGameObject;
    }

    public CollectableObjectData GetCollectableObjectData()
    {
        return CollectableObjectService.GetCollectableObjectData(CollectableObjectId);      
    }

    public PoolType GetStrogeType(PoolType storageType)
    {
        CollectableObjectData objectData = GetCollectableObjectData();

        if (storageType == objectData.CarryType) return objectData.CarryType;

        else if (storageType == objectData.FoldedType) return objectData.FoldedType;

        else if (storageType == objectData.HangedType) return objectData.HangedType;

        else return PoolType.Undefined;
    }
}