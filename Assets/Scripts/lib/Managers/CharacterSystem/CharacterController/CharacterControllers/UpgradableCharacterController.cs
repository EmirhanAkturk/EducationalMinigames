using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class UpgradableCharacterController : BasicCharacterController
{   public int Level
    {
        get => level;
        private set => level = value;
    }
    [SerializeField] private int level;

    // public override T GetCharacterValue<T>(CharacterDataType characterDataType)
    // {
    //     T value = (T)characterData.GetData(characterDataType);
    //     
    //     if (value is float)
    //     {
    //         float tempValue = (float)(object)value;
    //         tempValue = UpgradeManager.Instance.GetUpgradedValue(this, characterData.PoolType, characterDataType, tempValue);
    //         value = (T)(object)tempValue;
    //     }
    //     else if(value is CarryPointsCapacities)
    //     {
    //         CarryPointsCapacities newCapacities = new CarryPointsCapacities();
    //         var carryPointsCapacities = (CarryPointsCapacities)(object)value;
    //
    //         newCapacities.BackCapacity = (int)UpgradeManager.Instance.GetUpgradedValue(this, characterData.PoolType, characterDataType, carryPointsCapacities.BackCapacity);
    //         newCapacities.ElbowsCapacity = (int)UpgradeManager.Instance.GetUpgradedValue(this, characterData.PoolType, characterDataType, carryPointsCapacities.ElbowsCapacity);
    //         newCapacities.HandsCapacity = (int)UpgradeManager.Instance.GetUpgradedValue(this, characterData.PoolType, characterDataType, carryPointsCapacities.HandsCapacity);
    //
    //         value = (T)(object)newCapacities;
    //     }
    //         
    //     return value;
    // }
}
