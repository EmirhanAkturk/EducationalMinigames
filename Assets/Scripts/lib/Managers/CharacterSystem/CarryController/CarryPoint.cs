using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarryPoint : MonoBehaviour
{
    public CarryPointType CarryPointType => carryPointType;
    public int Capacity => GetCarryPointCapacity();
    
    [SerializeField] private CarryPointType carryPointType;
    
    public List<GameObject> gameObjects = new List<GameObject>();
    protected readonly Dictionary<CollectableObjectType, List<CollectableObject>> carriedObjectsDictionary = new Dictionary<CollectableObjectType, List<CollectableObject>>();

    private CarryController CarryController => carryController ??= GetComponentInParent<CarryController>();
    private CarryController carryController;
    protected ICharacterController CharacterController => characterController ??= GetComponentInParent<ICharacterController>();
    private ICharacterController characterController;
    protected CarrierAnimationController AnimationController => animationController ? animationController : (animationController = GetComponentInParent<CarrierAnimationController>());
    private CarrierAnimationController animationController;

    protected virtual void OnEnable()
    {
        AnimationController.ChangeArmState(CarryPointType, false);
    }

    private void OnDisable()
    {
        foreach (var item in gameObjects)
        {
            Destroy(item);
        }
        gameObjects.Clear();
    }

    #region Add & Remove

    public virtual void AddCarriedObject(CollectableObject carriedObject)
    {
        if (IsFull())
        {
            Debug.Log("Carry Point full");
            return;
        }

        var collectableObjectType = CollectableObjectService.GetCollectableObjectData(carriedObject.CollectableObjectId).CollectableObjectType;
        if(!carriedObjectsDictionary.ContainsKey(collectableObjectType))
            carriedObjectsDictionary.Add(collectableObjectType, new List<CollectableObject>());
        
        carriedObjectsDictionary[collectableObjectType].Add(carriedObject);
        AdjustCarriedObject(carriedObject);
        AnimationController.ChangeArmState(CarryPointType, true);
    }

    protected virtual void AddRepresentationalObject(PoolType objectType, GameObject go)
    {
        gameObjects.Add(go);
    }

    public CollectableObject RemoveCarriedObject(CollectableObjectType collectableObjectType, int collectableObjectId, bool checkObjectId)
    {
        if(!carriedObjectsDictionary.ContainsKey(collectableObjectType)) return null;
        CollectableObject collectableObject = null;
        CollectableObjectData collectableObjectData = CollectableObjectService.GetCollectableObjectData(collectableObjectId);

        var carriedObjects = carriedObjectsDictionary[collectableObjectType].ToList();
        carriedObjects.Reverse();
        
        foreach (var carriedObject in carriedObjects)
        {
            if ( !checkObjectId || (carriedObject.CollectableObjectId == collectableObjectId &&
                 collectableObjectData.CollectableObjectType == collectableObjectType))
            {
                collectableObject = carriedObject;
                break;
            }
        }
        return collectableObject != null ? RemoveCarriedObject(collectableObject) : null;
    }

    public CollectableObject RemoveCarriedObject(CollectableObject carriedObject)
    {
        var collectableObjectType = CollectableObjectService.GetCollectableObjectData(carriedObject.CollectableObjectId).CollectableObjectType;
        if(!carriedObjectsDictionary.ContainsKey(collectableObjectType)||
           !carriedObjectsDictionary[collectableObjectType].Contains(carriedObject)) return null;
        
        carriedObjectsDictionary[collectableObjectType].Remove(carriedObject);
        if (GetAllCarriedObjects().Count == 0) AnimationController.ChangeArmState(CarryPointType, false);
        var representationObjectType = CollectableObjectService.GetCollectableObjectData(carriedObject.CollectableObjectId).CarryType;
        RemoveRepresentationalObject(representationObjectType, carriedObject.ActiveGameObject);
        carriedObject.ActiveGameObject = null;
        return carriedObject;
    }

    public List<CollectableObject> RemoveAllCarriedObjects()
    {
        var allCarriedObjects = GetAllCarriedObjects().ToList();
        
        foreach (var carriedObject in allCarriedObjects)
            RemoveCarriedObject(carriedObject);

        return allCarriedObjects;
    }

    protected virtual bool RemoveRepresentationalObject(PoolType objectType, GameObject go)
    {
        if ( go == null)
            throw new Exception("Failed to delete Representational Object");

        gameObjects.Remove(go);
        if(PoolingSystem.IsAvailable())
            PoolingSystem.Instance.Destroy(objectType, go);
        return true;
    }

    #endregion
    
    #region Get Functions
    
    public List<CollectableObject> GetAllCarriedObjects(PoolType carryType = PoolType.All)
    {
        List<CollectableObject> allCarriedObjects = new List<CollectableObject>();
        foreach (var carriedObjectList in carriedObjectsDictionary.Values)
        {
            foreach (var carriedObject in carriedObjectList)
            {
                if(carryType == PoolType.All || carriedObject.GetCollectableObjectData().FoldedType == carryType || carriedObject.GetCollectableObjectData().HangedType == carryType)
                    allCarriedObjects.Add(carriedObject);
            }
        }
        return allCarriedObjects;
    }    
    
    public List<CollectableObject> GetAllCarriedObjectsById(int objectId)
    {
        List<CollectableObject> allCarriedObjects = new List<CollectableObject>();
        foreach (var carriedObjectList in carriedObjectsDictionary.Values)
        {
            foreach (var carriedObject in carriedObjectList)
            {
                if(carriedObject.CollectableObjectId == objectId)
                    allCarriedObjects.Add(carriedObject);
            }
        }
        return allCarriedObjects;
    }

    public List<CollectableObject> GetCarriedObjects(CollectableObjectType collectableObjectType)
    {
        return carriedObjectsDictionary[collectableObjectType];
    }

    public virtual Vector3 GetTargetRotation(bool isAdded = false)
    {
        return transform.rotation.eulerAngles;
    }
    
    public virtual Vector3 GetTargetPosition()
    {
        return Vector3.zero;
    }
    
    private int GetCarryPointCapacity()
    {
        var capacities = CharacterController.GetCharacterValue<CarryPointsCapacities>(CharacterDataType.CarryPointsCapacities);
        switch (CarryPointType)
        {
            case CarryPointType.Back:
                return capacities.BackCapacity;
            
            case CarryPointType.LeftElbow:
            case CarryPointType.RightElbow:
                return capacities.ElbowsCapacity;

            case CarryPointType.Hands:
                return capacities.HandsCapacity;

            /*case CarryPointType.RightHand:
            case CarryPointType.LeftHand:
                return capacities.HandsCapacity;*/
        }

        return 0;
    }

    #endregion

    #region Other

    public void SetArmState(bool putItUp)
    {
        AnimationController.ChangeArmState(CarryPointType, putItUp);
    }
    
    public bool GetArmState()
    {
        return AnimationController.GetArmState(CarryPointType);
    }
    
    public bool IsFull()
    {
        return CarryController.GetCountByCarryPointType(ConvertCarryPointTypeToGeneralType(CarryPointType)) >= Capacity;
    }

    public bool IsEmpty()
    {
        return GetAllCarriedObjects().Count == 0;
    }

    protected virtual CollectableObject AdjustCarriedObject(CollectableObject carriedObject)
    {
        var representationalObject = carriedObject.ActiveGameObject;

        MatService.UpdateObjectWithGMat(representationalObject, carriedObject.ObjectMat);

        representationalObject.transform.parent = transform;
        representationalObject.transform.localPosition = Vector3.zero;
        AddRepresentationalObject(CollectableObjectService.GetCollectableObjectData(carriedObject.CollectableObjectId).CarryType, representationalObject);

        return carriedObject;
    }

    private CarryPointType ConvertCarryPointTypeToGeneralType(CarryPointType carryPointType)
    {
        switch (carryPointType)
        {
            case CarryPointType.Back:
                return CarryPointType.Back;
            
            case CarryPointType.RightShoulder:
            case CarryPointType.LeftShoulder:
                return CarryPointType.Shoulders;

            case CarryPointType.RightElbow:
            case CarryPointType.LeftElbow:
                return CarryPointType.Elbows;

            case CarryPointType.Hands:
                return CarryPointType.Hands;

                /* case CarryPointType.RightHand:
                 case CarryPointType.LeftHand:
                     return CarryPointType.Hands;*/
        }

        Debug.Log("Undefined Carry Point Type");
        return CarryPointType.Back;
    }

    #endregion
}
