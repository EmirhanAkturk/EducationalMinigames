using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class BasicAnimationController : MonoBehaviour, IAnimationController
{
    public Action<string> OnCustomEventComplete; 
        
    public Animator Animator
    {
        get
        {
            if (animator == null || animator.Equals(null))
            { 
                animator = GetComponent<Animator>();
            }
            return animator;
        }
    }

    protected Dictionary<AnimationType, string> animationKeys = new Dictionary<AnimationType, string>()
    {
        {AnimationType.SpecialIdle, "SpecialIdle"},
        {AnimationType.Idle, "Idle"},
        {AnimationType.Walk, "Walk"},
        {AnimationType.Work, "Work"},
        {AnimationType.Pick, "Pick"},
    };

    private Dictionary<AnimationType, float> animationLenghts = new Dictionary<AnimationType, float>();

    private Animator animator;
    protected AnimationType activeAnimation;
    public virtual void ChangeAnimation(AnimationType animationType, float time = -1)
   {
       Animator.enabled = true;
        foreach (var animationPair in animationKeys)
        {
            Animator.SetBool(animationPair.Value, false);
        }

        Animator.SetBool(animationKeys[animationType], true);
        activeAnimation = animationType;
        //SetAnimationSpeed(time);
   }

    public AnimationType GetActiveAnimation()
    {
        return activeAnimation;
    }

    protected void SetAnimationSpeed(float time)
    {
        if (time > 0)
        {
            var stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.length > time)
            {
                Animator.speed = stateInfo.length / time;
            }
        }
        else
        {
            Animator.speed = 1;
        }
    }

   public void StopAnimation()
   {
       Animator.enabled = false;
   }
   
   public void SubscribeCustomEvent(Action<string> _OnCustomEventComplete)
   {
       OnCustomEventComplete += _OnCustomEventComplete;
   }   
   
   public void UnSubscribeCustomEvent(Action<string> _OnCustomEventComplete)
   {
       OnCustomEventComplete -= _OnCustomEventComplete;
   }

   public virtual void PlayAnimation(AnimationType animationType, float time = -1)
    {
        activeAnimation = animationType;
        Animator.Play(animationKeys[animationType]);
        //SetAnimationSpeed(time);
    }

   public void OnCustomEvent(string s)
   {
       OnCustomEventComplete?.Invoke(s);
   }
}
