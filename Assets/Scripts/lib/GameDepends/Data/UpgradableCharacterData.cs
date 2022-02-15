using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CharacterData", menuName = "lib/GameDependent/UpgradableCharacterData", order = 1)]
public class UpgradableCharacterData : ScriptableObject
{
    [Header("General Variables")]
    public int TeamID;
    public PoolType PoolType;
    public CharacterType CharacterType;
    public bool IsMainCharacter;
    public float InteractRange;
    public float SightRange;

    [Header("LevelCharacterDatas")] 
    public List<LevelCharacterData> LevelCharacterDatas;
    
    public object GetData(CharacterDataType dataType, int level)
    {
        int levelIndex = level - 1;
        
        switch (dataType)
        {
            //General Variables
            case CharacterDataType.TeamID:
                return TeamID;
            case CharacterDataType.PoolType:
                return PoolType;     
            case CharacterDataType.CharacterType:
                return CharacterType;

            //Movement Variables
            case CharacterDataType.MoveSpeed:
                return LevelCharacterDatas[levelIndex].MoveSpeed;
            case CharacterDataType.StoppingDistance:
                return LevelCharacterDatas[levelIndex].StoppingDistance;

            //Interactable Variables
            case CharacterDataType.InteractRange:
                return InteractRange;
            case CharacterDataType.SightRange:
                return SightRange;

            default:
                return null;
        }
    }
}

[Serializable]
public class LevelCharacterData
{
    
    [Header("Movement Variables")]
    public float MoveSpeed;
    public float StoppingDistance;

}

