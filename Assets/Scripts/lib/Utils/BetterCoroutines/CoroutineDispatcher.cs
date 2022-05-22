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

	public new static Coroutine StartCoroutine(IEnumerator coroutine)
	{
		return Context.StartCoroutine(coroutine);
	}
	
	public new static void StopCoroutine(Coroutine coroutine)
	{
		Context.StopCoroutine(coroutine);
	}
	
	public static void ShutDown()
	{
		Context.StopAllCoroutines();
	}
	
	public static Coroutine ExecuteWithDelay(Action callback, float delay)
	{
		return StartCoroutine(ExecuteWithDelayCoroutine(callback, delay));
	}
	
	private static IEnumerator ExecuteWithDelayCoroutine(Action callback, float delay)
	{
		yield return new WaitForSeconds(delay);
		callback?.Invoke();
	}
}