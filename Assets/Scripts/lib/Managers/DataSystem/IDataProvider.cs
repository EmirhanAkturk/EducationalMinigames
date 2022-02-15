using System.Collections.Generic;

public interface IDataProvider
{
    GameData LoadData();
    void SyncData(GameData gameData, bool isForced);
}