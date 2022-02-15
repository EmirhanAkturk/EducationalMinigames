using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

//TODO: Using reflection here which makes this whole system slow, ideally re implement the whole event system from scratch
//TODO: Make a custom property drawer to allow users add persistent events through the inspector
namespace HeroGames
{
	public static class PriorityEventTools
	{
		private static FieldInfo m_callsFieldInfo;
		private static PropertyInfo m_countProp;
		private static void Init()
		{
			if (!isDirty) return;
			m_callsFieldInfo = typeof(UnityEventBase).GetField("m_Calls", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
			m_countProp = m_callsFieldInfo.FieldType.GetProperty("Count");
			isDirty = false;
		}
		private static bool isDirty = true;
		internal static int GetListenerCount(UnityEventBase unityEvent)
		{
			Init();
			var invokeCallList = m_callsFieldInfo.GetValue(unityEvent);
			return (int)m_countProp.GetValue(invokeCallList);
		}
	}

	public class PriorityEvent
	{
		private readonly UnityEvent[] events;
		private int levelsCount;
		public PriorityEvent()
		{
			levelsCount = Enum.GetValues(typeof(PriorityLevel)).Length;
			events = new UnityEvent[levelsCount];
			for (int i = 0; i < levelsCount; i++)
				events[i] = new UnityEvent();
		}
		public void AddListener(UnityAction call, PriorityLevel priorityLevel = PriorityLevel.Default)
		{
			if (call == null) return;
			events[(int)priorityLevel].AddListener(call);
		}
		public void RemoveListener(UnityAction call)
		{
			if (call == null) return;
			for (int i = 0; i < levelsCount; i++)
			{
				if (events[i] == null) continue;
				int prevListenerCount = PriorityEventTools.GetListenerCount(events[i]);
				events[i].RemoveListener(call);
				int newListenerCount = PriorityEventTools.GetListenerCount(events[i]);
				if (newListenerCount != prevListenerCount)
					return;
			}
		}
		public void Invoke()
		{
			for (int i = levelsCount - 1; i >= 0; i--)
				events[i]?.Invoke();
		}
		public void Clear()
		{
			for (int i = 0; i < levelsCount; i++)
				events[i] = new UnityEvent();
		}
	}

	public class PriorityEvent<T0>
	{
		private readonly UnityEvent<T0>[] events;
		private int levelsCount;
		public PriorityEvent()
		{
			levelsCount = Enum.GetValues(typeof(PriorityLevel)).Length;
			events = new UnityEvent<T0>[levelsCount];
			for (int i = 0; i < levelsCount; i++)
				events[i] = new UnityEvent<T0>();
		}
		public void AddListener(UnityAction<T0> call, PriorityLevel priorityLevel = PriorityLevel.Default)
		{
			if (call == null) return;
			events[(int)priorityLevel].AddListener(call);
		}
		public void RemoveListener(UnityAction<T0> call)
		{
			if (call == null) return;
			for (int i = 0; i < levelsCount; i++)
			{
				if (events[i] == null) continue;
				int prevListenerCount = PriorityEventTools.GetListenerCount(events[i]);
				events[i].RemoveListener(call);
				int newListenerCount = PriorityEventTools.GetListenerCount(events[i]);
				if (newListenerCount != prevListenerCount)
					return;
			}
		}
		public void Invoke(T0 arg0)
		{
			for (int i = levelsCount - 1; i >= 0; i--)
				events[i]?.Invoke(arg0);
		}
		public void Clear()
		{
			for (int i = 0; i < levelsCount; i++)
				events[i] = new UnityEvent<T0>();
		}
	}

	public class PriorityEvent<T0, T1>
	{
		private readonly UnityEvent<T0, T1>[] events;
		private int levelsCount;
		public PriorityEvent()
		{
			levelsCount = Enum.GetValues(typeof(PriorityLevel)).Length;
			events = new UnityEvent<T0, T1>[levelsCount];
			for (int i = 0; i < levelsCount; i++)
				events[i] = new UnityEvent<T0, T1>();
		}
		public void AddListener(UnityAction<T0, T1> call, PriorityLevel priorityLevel = PriorityLevel.Default)
		{
			if (call == null) return;
			events[(int)priorityLevel].AddListener(call);
		}
		public void RemoveListener(UnityAction<T0, T1> call)
		{
			if (call == null) return;
			for (int i = 0; i < levelsCount; i++)
			{
				if (events[i] == null) continue;
				int prevListenerCount = PriorityEventTools.GetListenerCount(events[i]);
				events[i].RemoveListener(call);
				int newListenerCount = PriorityEventTools.GetListenerCount(events[i]);
				if (newListenerCount != prevListenerCount)
					return;
			}
		}
		public void Invoke(T0 arg0, T1 arg1)
		{
			for (int i = levelsCount - 1; i >= 0; i--)
				events[i]?.Invoke(arg0, arg1);
		}
		public void Clear()
		{
			for (int i = 0; i < levelsCount; i++)
				events[i] = new UnityEvent<T0, T1>();
		}
	}

}