using System;
using System.Collections.Generic;
using UnityEngine;

public class HandCarryPoint : CarryPoint
{
    [SerializeField] private float representationObjXStartRot = 20f; // for left hand, if is right hand => -20
    [SerializeField] private float representationObjXRotOffset = -20f;
    [SerializeField] private float representationObjYRot = -90f;
    [SerializeField] private float representationObjZRot = -160f;

    private bool isLeftHand;

    private void Awake()
    {
        isLeftHand = (CarryPointType == CarryPointType.LeftHand);
        if (!isLeftHand)
        {
            representationObjXStartRot *= -1;
            representationObjXRotOffset *= -1;
        }
    }
    protected override CollectableObject AdjustCarriedObject(CollectableObject carriedObject)
    {
        base.AdjustCarriedObject(carriedObject);
        int count = carriedObjectsDictionary[CollectableObjectService.GetCollectableObjectData(carriedObject.CollectableObjectId).CollectableObjectType].Count;
        float objectXRot = representationObjXStartRot + representationObjXRotOffset * (count - 1);
        Vector3 objectRot = new Vector3(objectXRot, representationObjYRot, representationObjZRot);
        carriedObject.ActiveGameObject.transform.localRotation = Quaternion.Euler(objectRot);

        return carriedObject;
    }
}
