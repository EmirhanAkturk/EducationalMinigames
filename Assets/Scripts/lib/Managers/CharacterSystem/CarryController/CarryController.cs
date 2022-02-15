using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryController : MonoBehaviour
{
    #region Tutorial
    private Dictionary<CollectableObjectType, int> collectedObjectCounts = new Dictionary<CollectableObjectType, int>(); 
    private Dictionary<CollectableObjectType, int> droppedObjectCounts = new Dictionary<CollectableObjectType, int>(); 
    #endregion
    
    public bool IsInitialize;

    private CharacterType CharacterType => characterType ??= GetComponent<ICharacterController>().GetCharacterValue<CharacterType>(CharacterDataType.CharacterType);
    private CharacterType? characterType;
    private readonly Dictionary<CarryPointType, CarryPoint> carryPointsDictionary = new Dictionary<CarryPointType, CarryPoint>();
    private readonly List<CarryPointType> carryPointTypes = new List<CarryPointType>() {CarryPointType.Back, CarryPointType.Shoulders, CarryPointType.Elbows, CarryPointType.Hands};
    #region Init
    
    public void Initialize()
    {
        LoadCarryPoints();
    }

    private void LoadCarryPoints()
    {
        carryPointsDictionary.Clear();
        var carryPoints = GetComponentsInChildren<CarryPoint>(false);
        foreach (var carryPoint in carryPoints)
            carryPointsDictionary.Add(carryPoint.CarryPointType, carryPoint);
        IsInitialize = true;
    }
    
    #endregion

    #region Carry & Drop

    public bool CarryObject(CollectableObject _carriedObject)
    {
        CarryPoint carryPoint = GetCarryPoint(CollectableObjectService.GetCollectableObjectData(_carriedObject.CollectableObjectId).CarryPointsType);
        
        if (carryPoint.IsFull()) return false;
        
        CollectableObject carriedObject = new CollectableObject(_carriedObject);
        _carriedObject.ActiveGameObject = null;

        AddCarriedObject(carryPoint.CarryPointType, carriedObject);
        var objectData = CollectableObjectService.GetCollectableObjectData(carriedObject.CollectableObjectId);
        IncreaseCollectedObjectCount(objectData.CollectableObjectType);
        // CheckTutorialState(true);
        return true;
    }

    public List<CollectableObject> DropObject(CollectableObjectType collectableObjectType, int collectableObjectId , int count = 1, bool checkObjectId = true)
    {
        List<CollectableObject> droppedObjects = new List<CollectableObject>();
        for (int i = 0; i < count; i++)
        {
            foreach (var carryPointType in carryPointTypes)
            {   
                var carryPoint = GetCarryPoint(carryPointType, false);
                if(carryPoint == null) continue;
                if (checkObjectId && carryPoint.GetAllCarriedObjectsById(collectableObjectId).Count == 0) // If selected hand does not have the searched object, try the other hand. 
                    carryPoint = GetCarryPoint(carryPointType);
                
                var droppedObject = RemoveCarriedObject(carryPoint.CarryPointType, collectableObjectType, collectableObjectId, checkObjectId);
                if (droppedObject != null)
                {
                    droppedObjects.Add(droppedObject);
                    var objectData = CollectableObjectService.GetCollectableObjectData(droppedObject.CollectableObjectId);  
                    IncreaseDroppedObjectCount(objectData.CollectableObjectType);
                    // CheckTutorialState(false);
                    if(droppedObjects.Count >= count) return droppedObjects;
                }
            }
        }
        return droppedObjects;
    }

    public void DropAllCarriedObjects()
    {
        foreach (var carryPoint in carryPointsDictionary.Values)
            carryPoint.RemoveAllCarriedObjects();
    }
    
    #endregion

    #region Add & Remove & Get

    private void AddCarriedObject(CarryPointType carryPointType, CollectableObject carriedObject)
    {
        if(!carryPointsDictionary.ContainsKey(carryPointType)) return;
        carryPointsDictionary[carryPointType].AddCarriedObject(carriedObject);
    }

    private CollectableObject RemoveCarriedObject(CarryPointType carryPointType, CollectableObject carriedObject)
    {
        return carryPointsDictionary.ContainsKey(carryPointType)
            ? carryPointsDictionary[carryPointType].RemoveCarriedObject(carriedObject)
            : null;
    }

    private CollectableObject RemoveCarriedObject(CarryPointType carryPointType, CollectableObjectType collectableObjectType, int collectableObjectId, bool checkObjectId)
    {
        return carryPointsDictionary.ContainsKey(carryPointType) 
            ? carryPointsDictionary[carryPointType].RemoveCarriedObject(collectableObjectType, collectableObjectId, checkObjectId)   
            : null;
    }

    private List<CollectableObject> GetCarriedObjectsByCollectableObjectType(CollectableObjectType collectableObjectType)
    {
        List<CollectableObject> collectableObjects = new List<CollectableObject>();
        
        foreach (var carryPointType in carryPointsDictionary.Keys)
            collectableObjects.AddRange(GetCarriedObjects(carryPointType, collectableObjectType));
        
        return collectableObjects;
    }   
    
    private List<CollectableObject> GetCarriedObjects(CarryPointType carryPointType, CollectableObjectType collectableObjectType)
    {
        return !carryPointsDictionary.ContainsKey(carryPointType) 
            ? new List<CollectableObject>() 
            : carryPointsDictionary[carryPointType].GetCarriedObjects(collectableObjectType);
    }

    public List<CollectableObject> GetAllCarriedObjects(CarryPointType carryPointType, PoolType carryType = PoolType.All)
    {
        return !carryPointsDictionary.ContainsKey(carryPointType) 
            ? new List<CollectableObject>() 
            : carryPointsDictionary[carryPointType].GetAllCarriedObjects(carryType);
    }

    public List<CollectableObject> GetAllCarriedObjects(PoolType carryType = PoolType.All)
    {
        if (carryPointsDictionary.Count == 0) return new List<CollectableObject>();
        
        var allCarriedObjects = new List<CollectableObject>();

        foreach (var carryPointType in carryPointTypes)
            allCarriedObjects.AddRange(GetAllObjectsByCarryPointType(carryPointType, carryType));
        
        return allCarriedObjects;
    }

    public CarryPoint GetCarryPoint(CarryPointType carryPointType, bool getMin = true)
    {
        if (!CheckCarryPointAvailable(carryPointType)) return null;
        
        switch (carryPointType)
        {
            case CarryPointType.Back:
                return carryPointsDictionary[CarryPointType.Back];
            case CarryPointType.Shoulders:
                return CompareCarriedObjectsCount(CarryPointType.LeftShoulder, CarryPointType.RightShoulder, getMin)
                    ? carryPointsDictionary[CarryPointType.LeftShoulder]
                    : carryPointsDictionary[CarryPointType.RightShoulder];
            case CarryPointType.Elbows:
                return CompareCarriedObjectsCount(CarryPointType.LeftElbow, CarryPointType.RightElbow, getMin)
                    ? carryPointsDictionary[CarryPointType.LeftElbow]
                    : carryPointsDictionary[CarryPointType.RightElbow];
            case CarryPointType.Hands:
                return carryPointsDictionary[CarryPointType.Hands];
                /*return CompareCarriedObjectsCount(CarryPointType.LeftHand, CarryPointType.RightHand, getMin)        
                    ? carryPointsDictionary[CarryPointType.LeftHand]
                    : carryPointsDictionary[CarryPointType.RightHand];*/
        }

        return null;
    }

    public int GetCountByCarryPointType(CarryPointType carryPointType)
    {
        switch (carryPointType)
        {
            case CarryPointType.Back:
                return GetAllCarriedObjects(CarryPointType.Back).Count;

            case CarryPointType.Elbows:
                return GetAllCarriedObjects(CarryPointType.RightElbow).Count + GetAllCarriedObjects(CarryPointType.LeftElbow).Count;

            case CarryPointType.Hands:
                return GetAllCarriedObjects(CarryPointType.Hands).Count;
                /*return GetAllCarriedObjects(CarryPointType.RightHand).Count + GetAllCarriedObjects(CarryPointType.LeftHand).Count;*/

            case CarryPointType.Shoulders:
                return GetAllCarriedObjects(CarryPointType.RightShoulder).Count + GetAllCarriedObjects(CarryPointType.LeftShoulder).Count;
            default:
                return 0;
        }
    }


    public List<CollectableObject> GetAllObjectsByCarryPointType(CarryPointType carryPointType, PoolType carryType = PoolType.All)
    {
        List<CollectableObject> allCarriedObjectsByPointType = new List<CollectableObject>();

        switch (carryPointType)
        {
            case CarryPointType.Back:
                allCarriedObjectsByPointType.AddRange(GetAllCarriedObjects(CarryPointType.Back, carryType));
                break;

            case CarryPointType.Shoulders:
                allCarriedObjectsByPointType.AddRange(GetAllCarriedObjects(CarryPointType.LeftShoulder, carryType));
                allCarriedObjectsByPointType.AddRange(GetAllCarriedObjects(CarryPointType.RightElbow, carryType));
                break;
            
            case CarryPointType.Elbows:
                allCarriedObjectsByPointType.AddRange(GetAllCarriedObjects(CarryPointType.RightElbow, carryType));
                allCarriedObjectsByPointType.AddRange(GetAllCarriedObjects(CarryPointType.LeftElbow, carryType));
                break;
            
            case CarryPointType.Hands:
                allCarriedObjectsByPointType.AddRange(GetAllCarriedObjects(CarryPointType.Hands, carryType));
                /*allCarriedObjectsByPointType.AddRange(GetAllCarriedObjects(CarryPointType.LeftHand, carryType));
                allCarriedObjectsByPointType.AddRange(GetAllCarriedObjects(CarryPointType.RightHand, carryType));*/
                break;

            default:
                return new List<CollectableObject>();
        }

        return allCarriedObjectsByPointType;
    }

    public List<CollectableObject> GetAllCarriedObjectById(CarryPointType carryPointType, int objectId)
    {
        List<CollectableObject> allCarriedObjectsByPointType = new List<CollectableObject>();

        switch (carryPointType)
        {
            case CarryPointType.Back:
                allCarriedObjectsByPointType.AddRange(GetAllObjectsById(CarryPointType.Back, objectId));
                break;

            case CarryPointType.Shoulders:
                allCarriedObjectsByPointType.AddRange(GetAllObjectsById(CarryPointType.LeftShoulder, objectId));
                allCarriedObjectsByPointType.AddRange(GetAllObjectsById(CarryPointType.RightElbow, objectId));
                break;
            
            case CarryPointType.Elbows:
                allCarriedObjectsByPointType.AddRange(GetAllObjectsById(CarryPointType.RightElbow, objectId));
                allCarriedObjectsByPointType.AddRange(GetAllObjectsById(CarryPointType.LeftElbow, objectId));
                break;
            
            case CarryPointType.Hands:
                allCarriedObjectsByPointType.AddRange(GetAllObjectsById(CarryPointType.Hands, objectId));
                /*allCarriedObjectsByPointType.AddRange(GetAllObjectsById(CarryPointType.LeftHand, objectId));
                allCarriedObjectsByPointType.AddRange(GetAllObjectsById(CarryPointType.RightHand,objectId));*/
                break;
            
            default:
                return new List<CollectableObject>();
        }

        return allCarriedObjectsByPointType;
    }

    private List<CollectableObject> GetAllObjectsById(CarryPointType carryPointType, int objectId)
    {
        return !carryPointsDictionary.ContainsKey(carryPointType) 
            ? new List<CollectableObject>() 
            : carryPointsDictionary[carryPointType].GetAllCarriedObjectsById(objectId);
    }

    public bool CheckIsFull(CarryPointType carryPointType)
    {
        switch (carryPointType)
        {
            case CarryPointType.Back:
                return carryPointsDictionary[CarryPointType.Back].IsFull();

            case CarryPointType.Elbows:
                return carryPointsDictionary[CarryPointType.RightElbow].IsFull() && carryPointsDictionary[CarryPointType.LeftElbow].IsFull();

            case CarryPointType.Hands:
                return carryPointsDictionary[CarryPointType.Hands].IsFull();
                /*return carryPointsDictionary[CarryPointType.RightHand].IsFull() && carryPointsDictionary[CarryPointType.LeftHand].IsFull();*/

            case CarryPointType.Shoulders:
                return carryPointsDictionary[CarryPointType.LeftShoulder].IsFull() && carryPointsDictionary[CarryPointType.RightShoulder].IsFull();

            default:
                return true;
        }
    }

    public bool CheckIsEmpty(CarryPointType carryPointType)
    {
        switch (carryPointType)
        {
            case CarryPointType.Back:
                return carryPointsDictionary[CarryPointType.Back].IsEmpty();

            case CarryPointType.Elbows:
                return carryPointsDictionary[CarryPointType.RightElbow].IsEmpty() && carryPointsDictionary[CarryPointType.LeftElbow].IsEmpty();

            case CarryPointType.Hands:
                return carryPointsDictionary[CarryPointType.Hands].IsEmpty();
                /*return carryPointsDictionary[CarryPointType.RightHand].IsEmpty() && carryPointsDictionary[CarryPointType.LeftHand].IsEmpty();*/

            case CarryPointType.Shoulders:
                return carryPointsDictionary[CarryPointType.LeftShoulder].IsEmpty() && carryPointsDictionary[CarryPointType.RightShoulder].IsEmpty();

            default:
                return true;
        }
    }

    private bool CheckCarryPointAvailable(CarryPointType carryPointType)
    {
        switch (carryPointType)
        {
            case CarryPointType.Back:
                return carryPointsDictionary.ContainsKey(CarryPointType.Back);
            case CarryPointType.Shoulders:
                return (carryPointsDictionary.ContainsKey(CarryPointType.LeftShoulder) ||
                        carryPointsDictionary.ContainsKey(CarryPointType.RightShoulder));
            case CarryPointType.Elbows:
                return (carryPointsDictionary.ContainsKey(CarryPointType.RightElbow) ||
                        carryPointsDictionary.ContainsKey(CarryPointType.LeftElbow));
            case CarryPointType.Hands:
                return carryPointsDictionary.ContainsKey(CarryPointType.Hands);
                /*return (carryPointsDictionary.ContainsKey(CarryPointType.LeftHand) ||
                        carryPointsDictionary.ContainsKey(CarryPointType.RightHand));*/
        }

        return false;
    }

    #endregion

    #region Other

    private void IncreaseCollectedObjectCount(CollectableObjectType collectableObjectType)
    {
        if (!collectedObjectCounts.ContainsKey(collectableObjectType)) collectedObjectCounts[collectableObjectType] = 0;
        ++collectedObjectCounts[collectableObjectType];
    }    
    
    private void IncreaseDroppedObjectCount(CollectableObjectType collectableObjectType)
    {
        if (!droppedObjectCounts.ContainsKey(collectableObjectType)) droppedObjectCounts[collectableObjectType] = 0;
        ++droppedObjectCounts[collectableObjectType];
    }
    
    public int GetCollectedObjectCount(CollectableObjectType collectableObjectType)
    {
        return collectedObjectCounts.ContainsKey(collectableObjectType)
            ? collectedObjectCounts[collectableObjectType]
            : 0;
    }
    
    public int GetDroppedObjectCount(CollectableObjectType collectableObjectType)
    {
        return droppedObjectCounts.ContainsKey(collectableObjectType)
            ? droppedObjectCounts[collectableObjectType]
            : 0;
    }
        
    private bool CompareCarriedObjectsCount(CarryPointType type1, CarryPointType type2, bool getMin)
    {
        int type1Count = GetAllCarriedObjects(type1).Count;
        int type2Count = GetAllCarriedObjects(type2).Count;
        if(getMin) return type1Count <= type2Count;
        return type1Count > type2Count;
    }

    // private void CheckTutorialState(bool isCarried)
    // {
    //     if(CharacterType != CharacterType.Saler) return;
    //     
    //     if(isCarried && !TutorialManager.Instance.IsTutorialCompleted(TutorialType.CollectSourceTutorial) &&
    //        CollectSourceTutorial.CheckTutorialCompleteState(GetCollectedObjectCount(CollectableObjectType.Raw)))
    //     {
    //         return;
    //     }
    //     else if (!isCarried && !TutorialManager.Instance.IsTutorialCompleted(TutorialType.DropSourceTutorial) &&
    //         DropSourceTutorial.CheckTutorialCompleteState(GetDroppedObjectCount(CollectableObjectType.Raw)))
    //     {
    //         return;
    //     }
    // }

    #endregion
}
