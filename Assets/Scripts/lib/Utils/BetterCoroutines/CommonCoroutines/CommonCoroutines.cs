using System;
using System.Collections;
using UnityEngine;

public static class CommonCoroutines
{
	public static IEnumerator SetActivateDelay(GameObject gameObject, bool activate, float delay)
	{
		yield return new WaitForSeconds(delay);
		gameObject.SetActive(activate);
	}
}