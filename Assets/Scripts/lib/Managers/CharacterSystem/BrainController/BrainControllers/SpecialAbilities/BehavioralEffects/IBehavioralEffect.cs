using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehavioralEffect
{
    bool Do();
    BehavioralEffectType BehavioralEffectType { get; }
}
