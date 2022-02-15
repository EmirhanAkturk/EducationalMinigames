using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBehavioralEffect : MonoBehaviour, IBehavioralEffect
{
    public abstract bool Do();
    public abstract BehavioralEffectType BehavioralEffectType { get; }

    protected const float RepeatDuration = 0.1f;

    protected BasicCharacterController CharacterController
    {
        get
        {
            if(characterController == null)
            {
                characterController = GetComponent<BasicCharacterController>();
            }
            return characterController;
        }
    }

    private BasicCharacterController characterController;

    protected virtual void OnEnable()
    {
        if (!CharacterController.BehavioralEffects.ContainsKey(BehavioralEffectType))
        {
            CharacterController.BehavioralEffects[BehavioralEffectType] = this;
        }
    }

    protected virtual void OnDisable()
    {
        if (CharacterController.BehavioralEffects.ContainsKey(BehavioralEffectType))
        {
            CharacterController.BehavioralEffects.Remove(BehavioralEffectType);
        }
    }
}

public enum BehavioralEffectType
{
    HealAura =1,
    BoostAura = 2,
    Fear = 3,
    ContinousDamage = 4,
    ContinousHeal = 5,
    Slow = 6,
    DeathSpawn = 7,
    Stun = 8,
    Push = 9,
    Charm = 10,
    Pull = 11,
    Rage = 12,
}
