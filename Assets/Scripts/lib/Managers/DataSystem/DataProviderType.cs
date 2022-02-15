using System.Collections.Generic;

public enum DataProviderType
{
    PlayerPref
}

public static class DataProviders
{

    public static Dictionary<DataProviderType, IDataProvider> DataProviderDic = new Dictionary<DataProviderType, IDataProvider>()
    {
        {DataProviderType.PlayerPref, new  PlayerPrefDataProvider()}
    };
}
