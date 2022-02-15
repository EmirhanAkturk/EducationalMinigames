using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemData
{
    #region Item Manager
    public InventoryType InventoryType;
    public int ItemId;
    [HideInInspector]
    public string UniqueId; // an Id which is spesific to an instance of item.
    public bool IsActive;
    #endregion
    
    #region Unlock Manager

    public int UnlockWorld;
    #endregion

    #region Minion

    // UI 
    public PoolType PoolType;
    public int CurrentLevel; // In game level
    public string Name;
    [JsonIgnore] public Sprite Sprite;
    public float Cooldown;
    public int ManaCost;
    public float CurrentHealth;
    public int Rank; //Battle Deck level
    // public RarityType RarityType;
    public string UnlockInfo;

    [Header("Special Ability")]
    public List<SpecialAbility> SpecialAbilities = new List<SpecialAbility>();
    
    [Header("Character Data")]
    public UpgradableCharacterData CharacterData;
    public Dictionary<CharacterDataType, List<BoostBlock>> BoostDictionary;

    [Header("Rank Data")]
    public List<BoostBlockList> RankBoosts = new List<BoostBlockList>();

    #endregion

}

public class JsonIgnoreAttribute : Attribute
{
}

public class ItemCardData
{
    public int ItemId;
    public PoolType PoolType;
    public int CurrentLevel;
    public string Name;
    [JsonIgnore] public Sprite Sprite;
    public float Cooldown;
    public int ManaCost;

    public ItemCardData(ItemData itemData)
    {
        ItemId = itemData.ItemId;
        PoolType = itemData.PoolType;
        CurrentLevel = itemData.CurrentLevel;
        Name = itemData.Name;
        Sprite = itemData.Sprite;
        Cooldown = itemData.Cooldown;
        ManaCost = itemData.ManaCost;
    }
}


[Serializable]
public class SpecialAbility
{
    [JsonIgnore] public Sprite SpecialAbilitySprite;
    public string SpecialAbilityTitle;
    public string SpecialAbilityDescription;
}

