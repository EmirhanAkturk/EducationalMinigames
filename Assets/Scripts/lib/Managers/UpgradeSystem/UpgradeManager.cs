using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    private UpgradeCollection UpgradeCollection;
    public Dictionary<int, UpgradeItem> UpgradeDictionary = new Dictionary<int, UpgradeItem>();
    
    //Level (floor) -> upgrade id, upgradeitem
    private Dictionary<int, Dictionary<int, UpgradeItem>> UpgradesPerLevelDict = new Dictionary<int, Dictionary<int, UpgradeItem>>();

    private const string COLLECTION_PATH = "Configurations/UpgradeCollection";
    private const string REMOTE_PATH = "Upgrades";
    private Dictionary<int, int> upgradeDatas = new Dictionary<int, int>();
    private List<TimedUpgradeData> timedUpgrades = new List<TimedUpgradeData>();
    private bool isLoaded = false;

    private void Load()
    {
        if (isLoaded) return;
        UpgradeCollection = Resources.Load<UpgradeCollection>(COLLECTION_PATH);
        UpgradeDictionary = UpgradeCollection.UpgradeDictionary;
        // var remotes = RemoteConfigManager.Instance.GetCustomTypeConfig<UpgradeRemoteData>(REMOTE_PATH);

        // if (remotes != null && remotes.List != null && remotes.List.Count > 0)
        // {
        //     foreach (var item in remotes.List)
        //     {
        //         if (UpgradeDictionary.ContainsKey(item.Id))
        //             UpgradeDictionary[item.Id].UpgradePrices = item.UpgradePrices;
        //     }
        // }

        LoadData();
    }

    private void LoadData()
    {
        var collectableData = DataService.GetData<Dictionary<InventoryType, Dictionary<int, int>>>(DataType.COLLECTABLE);
        if (collectableData.ContainsKey(InventoryType.Upgrade))
            upgradeDatas = collectableData[InventoryType.Upgrade];
        else
        {
            upgradeDatas = new Dictionary<int, int>();
            foreach (var upgradeId in UpgradeDictionary.Keys)
            {
                upgradeDatas[upgradeId] = 0;
            }
        }

        isLoaded = true;
    }

    private void SaveData()
    {
        if (!isLoaded) return;

        var collectableData = DataService.GetData<Dictionary<InventoryType, Dictionary<int, int>>>(DataType.COLLECTABLE);
        collectableData[InventoryType.Upgrade] = upgradeDatas;
        DataService.SetData(DataType.COLLECTABLE, collectableData);
    }

    public void Upgrade(int upgradeId, int price)
    {
        CheckTutorialState();

        Load();
        if (!upgradeDatas.ContainsKey(upgradeId)) upgradeDatas[upgradeId] = 1;
        else upgradeDatas[upgradeId]++;

        Dictionary<string, float> logParameters = new Dictionary<string, float>();
        logParameters.Add(UpgradeCollection.UpgradeDictionary[upgradeId].Name, upgradeDatas[upgradeId]);
        logParameters.Add(upgradeId.ToString(), upgradeId);

        // AnalyticsManager.Instance.CustomEvent("UpgradeStaff", logParameters);

        int value;
        ExchangeService.DoExchange(CurrencyType.Coin, -price, ExchangeMethod.ClampZero, out value);
        SaveData();
    }

    public void AddUpgradeToCharacter(ICharacterController characterController, int upgradeId, int upgradeLevel, float cooldown)
    {
        timedUpgrades.Add(new TimedUpgradeData()
        {
            CharacterController = characterController,
            UpgradeId = upgradeId,
            UpgradeLevel = upgradeLevel,
            EndTime = Time.time + cooldown
        });

        var cc = (characterController as BasicCharacterController);
        if (UpgradeCollection.UpgradeDictionary[upgradeId].CharacterDataType == CharacterDataType.MoveSpeed)
        {
            cc.SetSpeed(cc.GetCharacterValue<float>(CharacterDataType.MoveSpeed));
        }

        CameraManager.Instance.ShowWithCam(cc.gameObject, 3f);
    }

    public void RemoveUpgradeToCharacter(ICharacterController characterController)
    {
        List<TimedUpgradeData> upgrades = new List<TimedUpgradeData>();
        foreach (var timedUpgrade in timedUpgrades)
        {
            if (timedUpgrade.CharacterController != null &&
                timedUpgrade.CharacterController.Equals(characterController))
            {
                upgrades.Add(timedUpgrade);
            }
        }

        foreach (var upgrade in upgrades)
            timedUpgrades.Remove(upgrade);
    }

    public void AddTimedUpgrade(int upgradeId, int upgradeLevel, float cooldown)
    {
        timedUpgrades.Add(new TimedUpgradeData()
        {
            CharacterController = null,
            UpgradeId = upgradeId,
            UpgradeLevel = upgradeLevel,
            EndTime = Time.time + cooldown
        });
    }

    public bool HasTimedUpgrade(ICharacterController characterController)
    {
        foreach (var item in timedUpgrades)
        {
            if ((item.CharacterController == null && UpgradeCollection.UpgradeDictionary[item.UpgradeId].PoolType == characterController.GetCharacterValue<PoolType>(CharacterDataType.PoolType))
                || item.CharacterController == characterController)
                return true;
        }
        return false;
    }

    /*
    public float GetUpgradedValue(ICharacterController characterController, PoolType poolType, CharacterDataType characterDataType, float Value)
    {
        Load();
        float totalAdd = 0;
        float totalMult = 1;
        foreach (var item in upgradeDatas)
        {
            var allUpgrades = GetUpgradesPerLevel();
            if (!allUpgrades.ContainsKey(item.Key)) continue;
            var upgradeData = allUpgrades[item.Key];
            if (upgradeData.CharacterDataType == characterDataType && upgradeData.PoolType == poolType)
            {
                totalMult *= Mathf.Pow(upgradeData.Multiplier, item.Value);
                totalAdd += upgradeData.Addition * item.Value;
            }
        }

        var removedTimedItems = new List<TimedUpgradeData>();
        foreach (var item in timedUpgrades)
        {
            if (Time.time > item.EndTime)
            {
                removedTimedItems.Add(item);
                continue;
            }
            var upgradeData = UpgradeCollection.UpgradeDictionary[item.UpgradeId];
            // bool isSuper = item.CharacterController != null && StaffService.Instance.IsSuperStaff(item.CharacterController);
            if (upgradeData.CharacterDataType == characterDataType && ((!isSuper && item.CharacterController == null && upgradeData.PoolType == poolType) || item.CharacterController == characterController))
            {
                totalMult *= Mathf.Pow(upgradeData.Multiplier, item.UpgradeLevel);
                totalAdd += upgradeData.Addition * item.UpgradeLevel;
            }
        }

        foreach (var item in removedTimedItems)
        {
            timedUpgrades.Remove(item);
            // if (item.CharacterController != null) EventService.OnDeletedStaffBuff?.Invoke(item.CharacterController.GetCharacterValue<CharacterType>(CharacterDataType.CharacterType));
        }

        return Value * totalMult + totalAdd;
    }
    */

    public Dictionary<UpgradeItem, UpgradePrices> GetUpgradePrices()
    {
        Load();
        Dictionary<UpgradeItem, UpgradePrices> upgradeItems = new Dictionary<UpgradeItem, UpgradePrices>();

        foreach (var item in GetUpgradesPerLevel())
        {
            if (upgradeDatas.ContainsKey(item.Key))
            {
                if (upgradeDatas[item.Key] < item.Value.UpgradePrices.Count)
                    upgradeItems.Add(item.Value, item.Value.UpgradePrices[upgradeDatas[item.Key]]);
                else
                {
                    upgradeItems.Add(item.Value, item.Value.UpgradePrices[item.Value.UpgradePrices.Count - 1]);
                }

            }
            else
            {
                upgradeItems.Add(item.Value, item.Value.UpgradePrices[0]);
            }
        }

        return upgradeItems;
    }

    // public Dictionary<UpgradeItem, UpgradePrices> GetUpgradePricesByStationType(StationType stationType)
    // {
    //     Load();
    //     Dictionary<UpgradeItem, UpgradePrices> upgradeItems = new Dictionary<UpgradeItem, UpgradePrices>();
    //     foreach (var item in GetUpgradesPerLevel())
    //     {
    //         if (stationType == item.Value.StationType)
    //         {
    //             if (upgradeDatas.ContainsKey(item.Key))
    //             {
    //                 if (upgradeDatas[item.Key] < item.Value.UpgradePrices.Count)
    //                     upgradeItems.Add(item.Value, item.Value.UpgradePrices[upgradeDatas[item.Key]]);
    //                 else
    //                 {
    //                     upgradeItems.Add(item.Value, item.Value.UpgradePrices[item.Value.UpgradePrices.Count - 1]);
    //                 }
    //
    //             }
    //             else
    //             {
    //                 upgradeItems.Add(item.Value, item.Value.UpgradePrices[0]);
    //             }
    //         }
    //     }
    //
    //     return upgradeItems;
    // }

    private Dictionary<int, UpgradeItem> GetUpgradesPerLevel()
    {
        int level = LevelManager.Instance.ActiveLevelId; // Floor

        if (!UpgradesPerLevelDict.ContainsKey(level))
        {
            Dictionary<int, UpgradeItem> res = new Dictionary<int, UpgradeItem>();
            foreach (var item in UpgradeCollection.UpgradeDictionary)
            {
                if (item.Value.Level == level || item.Value.Level == -1)
                    res.Add(item.Key, item.Value);
            }
            UpgradesPerLevelDict[level] = res;
        }

        return UpgradesPerLevelDict[level];
    }

    public UpgradeItem GetUpgradeItemById(int upgradeId)
    {
        Load();
        return UpgradeDictionary.ContainsKey(upgradeId) ? UpgradeDictionary[upgradeId] : null;
    }

    public UpgradePrices GetPrice(UpgradeItem upgradeItem)
    {
        return UpgradeCollection.UpgradeDictionary[upgradeItem.Id].UpgradePrices[upgradeDatas[upgradeItem.Id]];
    }

    public UpgradePrices GetPrice(int itemId, int itemLevel)
    {
        return UpgradeCollection.UpgradeDictionary[itemId].UpgradePrices[itemLevel];
    }

    public UpgradePrices GetPriceById(int upgradeId)
    {
        return GetPrice(GetUpgradeItemById(upgradeId));
    }

    public int GetUpgradeLevel(int upgradeId)
    {
        if (!upgradeDatas.ContainsKey(upgradeId)) return 1;
        else return upgradeDatas[upgradeId] + 1;
    }

    public bool CheckCanUpgrade(UpgradeItem upgradeItem)
    {
        if (upgradeDatas == null || (!upgradeDatas.ContainsKey(upgradeItem.Id) && UpgradeCollection.UpgradeDictionary.ContainsKey(upgradeItem.Id)))
        {
            return true;
        }
        if (upgradeDatas[upgradeItem.Id] >= UpgradeCollection.UpgradeDictionary[upgradeItem.Id].UpgradePrices.Count)
        {
            return false;
        }
        return true;
    }

    private void CheckTutorialState()
    {
        // if (!TutorialManager.Instance.IsTutorialCompleted(TutorialType.UpgradeStaffTutorial))
        //     UpgradeStaffTutorial.CheckTutorialCompleteState();
    }

    // public bool HasAnyStaffUnBuffByType(CharacterType characterType)
    // {
    //     foreach (var item in StaffService.Instance.OwnedStaffs)
    //     {
    //         if (!StaffService.Instance.IsSuperStaff(item.Value) &&
    //             item.Value.GetCharacterValue<CharacterType>(CharacterDataType.CharacterType) == characterType &&
    //             !HasTimedUpgrade(item.Value))
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }
    //
    // public int GetBoostedStaffCountWCharacterType(CharacterType characterType)
    // {
    //     int count = 0;
    //     foreach (var item in StaffService.Instance.OwnedStaffs)
    //     {
    //         if (!StaffService.Instance.IsSuperStaff(item.Value) &&
    //             item.Value.GetCharacterValue<CharacterType>(CharacterDataType.CharacterType) == characterType &&
    //             HasTimedUpgrade(item.Value))
    //         {
    //             count++;
    //         }
    //     }
    //     return count;
    // }
}

public class TimedUpgradeData
{
    public int UpgradeId;
    public ICharacterController CharacterController;
    public int UpgradeLevel;
    public float EndTime;
}

