using System;
using System.Collections.Generic;
using UnityEngine.Events;

public enum ExchangeMethod
{
	UnChecked, // Does the exchange unconditionally
	ClampZero, // Clamps value to 0 if it is negative
	LimitZero // Does not allow to a negative result for exchange
}