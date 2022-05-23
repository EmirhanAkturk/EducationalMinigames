using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventService 
{
    public static readonly UnityEvent OnMoneyUpdate = new UnityEvent();
    public static readonly UnityEvent<MiniGameType> OnGameStart = new UnityEvent<MiniGameType>();
}
