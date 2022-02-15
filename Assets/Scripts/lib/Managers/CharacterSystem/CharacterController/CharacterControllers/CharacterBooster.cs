using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBooster : MonoBehaviour
{
    public Dictionary<CharacterDataType, List<BoostBlock>> BoostDictionary = new Dictionary<CharacterDataType, List<BoostBlock>>();

    public virtual float ApplyBoost(CharacterDataType characterDataType, float data)
    {
        if (!BoostDictionary.ContainsKey(characterDataType))
            return data;

        float totalMultiply = 1;
        float totalAddition = 0;

        List<BoostBlock> expiredBoosts = new List<BoostBlock>();

        foreach (var boostBlock in BoostDictionary[characterDataType])
        {
            if(boostBlock.EndTime > -1 && boostBlock.EndTime < Time.time)
            {
                expiredBoosts.Add(boostBlock);
                continue;
            }
            totalMultiply *= boostBlock.Multiply;
            totalAddition += boostBlock.Addition;
        }
        
        foreach (var item in expiredBoosts)
        {
            BoostDictionary[characterDataType].Remove(item);
        }

        data = data * totalMultiply + totalAddition;
        
        return data;
    }
    
    public void Boost(CharacterDataType characterDataType, BoostBlock boostBlock)
    {
        if (BoostDictionary.ContainsKey(characterDataType))
        {
            BoostDictionary[characterDataType].Add(boostBlock);
        }
        else
        {
            BoostDictionary.Add(characterDataType, new List<BoostBlock>(){boostBlock});
        }
    }

    public void ResetAllBoosts()
    {
        BoostDictionary.Clear();
    }
}

[Serializable]
public class BoostBlock
{
    public float Multiply = 1;
    public float Addition = 0;
    [HideInInspector]
    public float EndTime = -1;

    public BoostBlock(float multiply, float addition)
    {
        Multiply = multiply;
        Addition = addition;
    }
}

[Serializable]
public class TypedBoostBlock
{
    public CharacterDataType DataType;
    public BoostBlock Boost;

    public TypedBoostBlock(float multiply, float addition, CharacterDataType dataType)
    {
        Boost = new BoostBlock(multiply, addition);
        DataType = dataType;
    }
}