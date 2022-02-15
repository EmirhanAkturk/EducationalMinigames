using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ExchangeEvent : UnityEvent<CurrencyType, int, int> { }

public static class ExchangeService
{
	public static Dictionary<CurrencyType, ExchangeEvent> OnDoExchange = new Dictionary<CurrencyType, ExchangeEvent>();

	private static Dictionary<CurrencyType, int> exchangeData = new Dictionary<CurrencyType, int>();

	static ExchangeService()
	{
		if (!Application.isPlaying) return;
        Load();
	}
	private static void Load()
	{
		if (!DataService.IsReady) return;
		exchangeData = DataService.GetData<Dictionary<CurrencyType, int>>(DataType.CURRENCY);

		foreach (CurrencyType currencyType in Enum.GetValues(typeof(CurrencyType)))
			OnDoExchange.Add(currencyType, new ExchangeEvent());
	}

	public static bool DoExchange(CurrencyType currencyType, int amount, ExchangeMethod exchangeMethod, out int value)
	{
		value = GetValue(currencyType);

		if (exchangeMethod == ExchangeMethod.LimitZero && value + amount < 0) return false;
		else if (exchangeMethod == ExchangeMethod.ClampZero && value + amount < 0)
		{
			amount = -value;
			value = 0;
		}
		else
		{
			value += amount;
		}

		exchangeData[currencyType] = value;
		Save();
		OnDoExchange[currencyType].Invoke(currencyType, amount, value);
		// if(currencyType == CurrencyType.Coin && amount >= 0) TutorialManager.Instance.CheckMoneyRequiredTutorialsState();
		return true;
	}

	public static bool DoExchange(CurrencyType currencyType, int amount, int clampValue, out int value)
	{
		int tmpAmount = amount;
		if (GetValue(currencyType) + amount > clampValue)
		{
			tmpAmount = clampValue - GetValue(currencyType);
		}

		DoExchange(currencyType, tmpAmount, ExchangeMethod.UnChecked, out value);
		return true;
	}

	public static int GetValue(CurrencyType currencyType, int initialValue = 0)
	{
		if (!exchangeData.ContainsKey(currencyType))
		{
			exchangeData[currencyType] = initialValue;
		}
		return exchangeData[currencyType];
	}
	public static void SetValue(CurrencyType currencyType, int value)
	{
		exchangeData[currencyType] = value;
		Save();
	}

	private static void Save()
	{
		DataService.SetData(DataType.CURRENCY, exchangeData);
	}
}
