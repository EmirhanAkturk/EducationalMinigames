using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CInputSystem
{
	public static Vector2 GetPosition()
	{
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		return Input.GetTouch(0).position;
#else
		return Input.mousePosition;
#endif
	}

	public static bool GetInputDown()
	{
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		if (Input.GetTouch(0).phase == TouchPhase.Began)
			return true;
#else
		if (Input.GetMouseButtonDown(0))
			return true;
#endif

		return false;
	}

	public static bool GetInput()
	{
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
			return true;
#else
		if (Input.GetMouseButton(0))
			return true;
#endif
		return false;
	}

	public static bool GetInputUp()
	{
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		if (Input.GetTouch(0).phase == TouchPhase.Ended)
			return true;
#else
		if (Input.GetMouseButtonUp(0))
			return true;
#endif
		return false;
	}


}
