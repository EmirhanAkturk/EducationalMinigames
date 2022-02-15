using System;
using System.Collections;
using UnityEngine;

public class CoroutineDispatcher : MonoBehaviour
{
	private static MonoBehaviour _context;
	private static MonoBehaviour Context
	{
		get
		{
			if (_context == null)
			{
				_context = new GameObject().AddComponent<CoroutineDispatcher>();
				_context.name = "Coroutine Dispatcher";
			}
			return _context;
		}
	}

	public static new void StartCoroutine(IEnumerator coroutine)
	{
		Context.StartCoroutine(coroutine);
	}
	
	public static void StartCoroutine(Action action, float delay)
	{
		Context.StartCoroutine(DoWithDelay(action, delay));
	}

	private static IEnumerator DoWithDelay(Action action, float delay)
	{
		yield return new WaitForSeconds(delay);
		action?.Invoke();
	}
	
	public static new void StopCoroutine(IEnumerator coroutine)
	{
		Context.StopCoroutine(coroutine);
	}
	public static void ShutDown()
	{
		Context.StopAllCoroutines();
	}
}