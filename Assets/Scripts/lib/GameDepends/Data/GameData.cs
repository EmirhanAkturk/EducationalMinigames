using System;
using System.Collections.Generic;

public abstract class GameDataBlock{}


public enum DataType
{
    SETTING, //Required for SettingManager
    CURRENCY, // Required for ExchangeManager
    STATE, // Required for LevelManager
    INVENTORY, // Required for InventoryManager
    COLLECTABLE, // Required for CollectableManager
    UNLOCK, // Required for UnlockManager
    INTERACTABLE,
}

public class GameData
{
    //Burada dictionary kullanılmamasının nedeni, her data tipi farklı olduğu için, serialize işlemlerinde kayıp oluşmasını önlemektir.

    public Dictionary<SettingType, int> Setting = new Dictionary<SettingType, int>();
    public Dictionary<CurrencyType, int> Currency = new Dictionary<CurrencyType, int>();
    public Dictionary<StateType, int> State = new Dictionary<StateType, int>();
    public Dictionary<InventoryType, List<ItemData>> Inventory = new Dictionary<InventoryType, List<ItemData>>();
    public Dictionary<InventoryType, Dictionary<int, int>> Collectable = new Dictionary<InventoryType, Dictionary<int,int>>();
    public Dictionary<InventoryType, List<int>> Unlock = new Dictionary<InventoryType, List<int>>();
    public Dictionary<string, string> Interactions = new Dictionary<string, string>();

    public int PlayerLevel;

    public T GetData<T>(DataType DataName) where T : class, new()
    {
        switch (DataName)
        {
            case (DataType.SETTING): return Setting as T;
            case (DataType.CURRENCY): return Currency as T;
            case (DataType.STATE): return State as T;
            case (DataType.INVENTORY): return Inventory as T;
            case (DataType.COLLECTABLE): return Collectable as T;
            case (DataType.UNLOCK): return Unlock as T;
            case (DataType.INTERACTABLE): return Interactions as T;
            default:
                throw new Exception("The DataName: " + DataName + " does not exist in your data list in GetData");
        }
    }

    public void SetData<T>(DataType DataName, T Value) where T : class, new()
    {
        switch (DataName)
        {
            case (DataType.SETTING):
                Setting = Value as Dictionary<SettingType, int>;
                break;
            case (DataType.CURRENCY):
                Currency = Value as Dictionary<CurrencyType, int>;
                break;
            case (DataType.STATE):
                State = Value as Dictionary<StateType, int>;
                break;
            case (DataType.INVENTORY):
                Inventory = Value as Dictionary<InventoryType, List<ItemData>>;
                break;
            case (DataType.COLLECTABLE):
                Collectable = Value as Dictionary<InventoryType, Dictionary<int, int>>;
                break;
            case (DataType.UNLOCK):
                Unlock = Value as Dictionary<InventoryType, List<int>>;
                break;
            case (DataType.INTERACTABLE):
                Interactions = Value as Dictionary<string, string>;
                break;
            default:
                throw new Exception("The DataName: " + DataName + " does not exist in your data list in SetData");
        }
    }
}
