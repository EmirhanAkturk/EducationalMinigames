using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfigurations", menuName = "lib/GameConfigurations")]
public class GameConfigurations : ScriptableObject
{
	[Header("CPI")]
    public bool IsMoneyHorizontal = true;
    public float CollectMoneyAnimDuration = 0.125f;
    [Tooltip("Karakterin sırtında kaç sıra halinde stackleneceği")] public int CollectMoneyXSlotCapacity = 3;
    [Tooltip("Karakterin sırtında kaç sıra halinde stackleneceği")] public int CollectMoneyZSlotCapacity = 7;
    public float MoneyDropAnimDuration = 3f;
    public float AnimJumpPower = 2.5f;
    public int CostPerMoneyObject = 100;
    public int MinMoneyCount = 7;
    public int MaxMoneyCount = 21;
    public Ease TileUnlockAnimEase = Ease.OutQuad;
    [Tooltip("Paranın animasyon süresinin, yüzde kaçının hareket öncesi bekleme olacağı")] [Range(0, 100)] public int AnimMoveDelayRatio = 25;

    [Header("Money stack")]
    public int XDimension = 6;
    public int YDimension = 18;
    public int ZDimension = 6;
    public int MoneyAnimCount;
    [Tooltip("Sırtta 1 paranın kaç para temsil edeceği")] public int MoneyStackOnBack = 3;

    [Header("Tile Unlock Anim")]
    public float WaitBeforePayDelay = 3f;
    public float TileUnlockDuration = 1f;
    public float InteractionObjectUnlockDuration = 1f;

    //Required for DataManager
    public DataProviderType DataProviderType;
    //Required for LevelManager
    //public LevelBackupMethod LevelBackupMethod;

    [Header("Indicator")] 
    public float NewItemUnlockedTextHideDelay = 10;

    [Header("Customer Spawn")]
    public List<PoolType> CustomerPoolTypes;
    public float CustomerSpawnPeriod = 5f;
    [Range(0, 300)] public int CustomerCountRatio = 75;


    [Header("Debug")] 
    public bool CheckTutorial = true;


    [Header("Balancing")]
    public CameraType IngameCameraType = CameraType.InGameCamera;

    [Header("Tutorial")]
    public int RequiredRollCount = 5;

	[Header("Customer Parameters")]
	[Range(0, 100f)] public float goToTablePercentage;
	[Range(0, 100f)] public float GoToVendingMachinePercentage;

    [Header("Offline")]
    public int OLMoneyPerPeriodPerManifact = 1;
    public int OLMaxMoneyPerManifact = 50;
    public int OLMoneyPeriodMinute = 5;

    #region Temp

    // [Header("Balancing")]
    // public float GameSpeed = 1;
    // [Range(0, 100f)] public float globalCardDropRate = 50f;
    // public int RequiredStarPerCard = 5;
    // public bool EnableAutoMerge = false;

    //    [Header("Energy")]
    //    public int energyRequiredToPlay = 5;
    //    public int energyMaxAmount = 25;
    //    [Tooltip("Increase Energy amount every [EnergyFillRate] seconds")] public int energyFillRate = 300;
    //    [Tooltip("Increase Energy amount by [EnergyFillAmount]")] public int energyFillAmount = 10;
    //    
    //    [Header("Energy Boost")]
    // public int bigEnergyBoostCost = 60;
    // public int bigEnergyBoostAmount = 20;
    // public int smallEnergyBoostCost = 20;
    // public int smallEnergyBoostAmount = 5;
    //
    //
    //    [Header("Xp")]
    //    public int MaxXp;
    //    public float InGameAndAccountXpRate = 10f;
    //
    //    [Header("Idle")]
    //    public int idleIncome = 3; // TODO Assign from remote config
    //    public float incomePeriod = 7f; // May be assign from game config 
    //    public int maxIdleIncome = 500;  // TODO Assign from remote config
    //    public float accountLevelToIncomeMultiplier = 1; // TODO Assign from remote config
    //    public float accountLevelToMaxIncomeMultiplier = 100;  // TODO Assign from remote config
    //
    //    [Header("Attack Range")] 
    //    public float MeleeUpLimit = 3f;
    //    public float MediumUpLimit = 5f;
    //    
    //    [Header("Debug")]
    //    public bool IsForceLevel = false;
    //    public int ForcedWorld = -1;
    //    public int ForcedLevel = -1;
    //
    //    public bool IsForceWorldFolder = false;
    //    public string ForcedWorldFolder = "A";
    //    
    //    public bool IsForcedAbilityButton = false;
    //
    //    [Header("Chest")]
    //    public int MinutesForOneGem;
    #endregion

    [Header("CPI")] 
    public bool HideHealthBars = false;
    // public bool CPIFingerActive = false;

    [Header("Loggers")]
    public bool Facebook = false;

    public bool Elephant = false;

    public bool GameAnalytics = false;

    public bool FirebaseAnalytics = false;

    [Header("Community")]
	public string discordInviteLink;

    [Header("Money")]
    public int MoneyPerOneBale;

    [Header("Customer AI Rules")]
    [Tooltip("Bir üründen max kaç tane alabileceği")] public int maxProductCount;
    [Tooltip("Sadece 1 ürün için gelenlerin yüzdeliği")] public int oneProductPercentage;

    [Header("Shopping Cart")]
    [Tooltip("Karakterin arabada kaç sıra halinde stackleneceği")] public int CollectProductXSlotCapacity = 3;
    [Tooltip("Karakterin arabada kaç sıra halinde stackleneceği")] public int CollectProductZSlotCapacity = 7;


}