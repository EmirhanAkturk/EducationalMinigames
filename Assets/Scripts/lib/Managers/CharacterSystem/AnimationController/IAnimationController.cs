using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationController
{
    Animator Animator { get; }
    void SubscribeCustomEvent(Action<string> OnCustomEventComplete);
    void UnSubscribeCustomEvent(Action<string> OnCustomEventComplete);
    void ChangeAnimation(AnimationType animationType, float time = -1);
    void PlayAnimation(AnimationType animationType, float time = -1);
    AnimationType GetActiveAnimation();
    void StopAnimation();
}
