using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAnimationController : BasicAnimationController
{
    public override void ChangeAnimation(AnimationType animationType, float time = -1)
    {
        if (animationType == AnimationType.Special)
        {
            Animator.enabled = true;

            foreach (var animationPair in animationKeys)
            {
                Animator.SetBool(animationPair.Value, false);
            }
            Animator.SetBool("Special", true);
            activeAnimation = animationType;
            SetAnimationSpeed(time);
        }
        else
        {
            Animator.SetBool("Special", false);
            base.ChangeAnimation(animationType, time);
        }
    }

    public override void PlayAnimation(AnimationType animationType, float time = -1)
    {
        activeAnimation = animationType;
        Animator.Play(animationType == AnimationType.Special? "Special": animationKeys[animationType]);
        SetAnimationSpeed(time);
    }
}
