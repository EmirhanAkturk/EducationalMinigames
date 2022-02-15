using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class DataService
{
	public static bool IsReady { get; private set; }

	private static bool isSuspicious = false;
	private static IDataProvider dataProvider;
	private static GameData data = new GameData();
	
	static DataService()
	{
		if (!Application.isPlaying) return;
		dataProvider = DataProviders.DataProviderDic[ConfigurationService.Configurations.DataProviderType];
		data = dataProvider.LoadData();
		IsReady = true;

		//Register Unity Focus and quit events
		Application.focusChanged += (focused) => { if (!focused) Save(); };
		Application.quitting += () => Save();
	}

	public static GameData GetData()
	{
		if (!IsReady)
		{
			isSuspicious = true;
			Debug.LogError("DataManager: GetData,  Reaching Data too Early");
		}
		return data;
	}

	public static T GetData<T>(DataType DataName) where T : class, new()
	{
		if (!IsReady)
		{
			isSuspicious = true;
			Debug.LogError("DataManager: GetData,  Reaching Data too Early");
		}
		return data.GetData<T>(DataName);
	}

	public static void SetData<T>(DataType DataName, T Value) where T : class, new()
	{
		if (!IsReady)
		{
			Debug.LogError("DataManager: SetData,  Reaching Data too Early");
		}
		if (isSuspicious)
		{
			Debug.LogError("DataManager: SetData,  Writing suspicious data DataName; OldData: "
				+ Newtonsoft.Json.JsonConvert.SerializeObject(data.GetData<T>(DataName)) + ", NewData"
				+ Newtonsoft.Json.JsonConvert.SerializeObject(Value)
				);
		}
		data.SetData(DataName, Value);
	}

	private static void Save()
	{
		dataProvider.SyncData(data, true);
	}

}