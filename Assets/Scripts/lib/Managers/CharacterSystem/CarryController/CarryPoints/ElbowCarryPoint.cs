using System;
using System.Collections.Generic;
using UnityEngine;

public class ElbowCarryPoint : CarryPoint
{
    [SerializeField] private float representationObjXStartRot = -170f; // for left hand, if is right hand => -20
    [SerializeField] private float representationObjXRotOffset = -20f;
    [SerializeField] private float representationObjYRot = 70f;
    [SerializeField] private float representationObjZRot = -20f;

    private bool isLeftElbow;

    private void Awake()
    {
        isLeftElbow = (CarryPointType == CarryPointType.LeftElbow);
        // if (!isLeftElbow)
        // {
        //     representationObjXStartRot *= -1;
        //     representationObjXRotOffset *= -1;
        // }
    }

    public override Vector3 GetTargetRotation(bool isAdded = false)
    {
        int count = GetAllCarriedObjects().Count;
        count += isAdded ? 0 : 1;
    
        float objectXRotOffset = representationObjXRotOffset * (count % 2 != 0 ? (count - 1) : (count * -1));
        Vector3 objectRot = new Vector3(representationObjXStartRot + objectXRotOffset, representationObjYRot, representationObjZRot);
        return objectRot;
    }

    protected override CollectableObject AdjustCarriedObject(CollectableObject carriedObject)
    {
        base.AdjustCarriedObject(carriedObject);
        Vector3 objectRot = GetTargetRotation(true);
        carriedObject.ActiveGameObject.transform.localRotation = Quaternion.Euler(objectRot);

        return carriedObject;
    }
}
