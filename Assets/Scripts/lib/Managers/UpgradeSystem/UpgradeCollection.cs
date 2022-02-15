using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "lib/Upgrade Collection", fileName = "UpgradeCollection")]
public class UpgradeCollection: ScriptableObject
{
    [SerializeField] private List<UpgradeItem> Items = new List<UpgradeItem>();
    private Dictionary<int, UpgradeItem> upgradeDictionary = new Dictionary<int, UpgradeItem>();

    public Dictionary<int, UpgradeItem> UpgradeDictionary
    {
        get
        {
            if (!isLoaded) Load();
                
            return upgradeDictionary;
        }
    }

    [NonSerialized] private bool isLoaded;

    private void Load()
    {
        foreach (var item in Items)
        {
            if (item.IsActive)
            {
                upgradeDictionary[item.Id] = item;
            }
        }
        isLoaded = true;
    }
}

[Serializable]
public class UpgradeItem
{
    public int Id;
    public string Name;
    public int Level = 1;
    public bool IsActive;
    public PoolType PoolType;
    public Sprite PersonelSprite;
    public Sprite UpgradeIconSprite;
    public CharacterDataType CharacterDataType;
    // public StationType StationType;
    public string UpgradeMessage;
    public float Multiplier;
    public float Addition;
    public List<UpgradePrices> UpgradePrices = new List<UpgradePrices>();
}

[Serializable]
public class UpgradePrices
{
    public int PricesWithAd;
    public int PricesWitoutAd;
}

public class UpgradeRemoteItem
{
    public int Id;
    public string Name;
    public int Level = 1;
    public List<UpgradePrices> UpgradePrices = new List<UpgradePrices>();
}

public class UpgradeRemoteData
{
    public List<UpgradeRemoteItem> List = new List<UpgradeRemoteItem>();
}