using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankedCharacterBooster : CharacterBooster
{
    private ICharacterController CharacterController
    {
        get
        {
            if(characterController == null)
            {
                characterController = GetComponent<ICharacterController>();
            }
            return characterController;
        }
    }
    private ICharacterController characterController;


    private PoolType PoolType
    {
        get
        {
            return CharacterController.GetCharacterValue<PoolType>(CharacterDataType.PoolType);
        }
    }

    private ItemData ItemData
    {
        get
        {
            if(itemData == null)
            {
                //itemData = InventoryManager.Instance.GetItemByPoolType(PoolType);
            }
            return itemData;
        }
    }

    private ItemData itemData;

    public override float ApplyBoost(CharacterDataType characterDataType, float data)
    {
        float totalMuliply = 1;
        float totalAddition = 0;
        if (ItemData != null)
        {
            for (int i = 0; i < ItemData.Rank - 1; i++)
            {
                if (i >= ItemData.RankBoosts.Count) break;
                foreach (var boost in ItemData.RankBoosts[i].List)
                {
                    if (boost.DataType == characterDataType)
                    {
                        totalMuliply *= boost.Boost.Multiply;
                        totalAddition += boost.Boost.Addition;
                    }
                }
            }
        }

        data = data * totalMuliply + totalAddition;

        return base.ApplyBoost(characterDataType, data);
    }
}

[Serializable]
public class BoostBlockList
{
    public List<TypedBoostBlock> List = new List<TypedBoostBlock>();
}
