using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierAnimationController : BasicAnimationController
{
    
    #region Carriers

    public virtual void ChangeArmState(CarryPointType handType, bool putItUp)
    {
        if( handType != CarryPointType.LeftHand && handType != CarryPointType.LeftElbow && 
            handType != CarryPointType.RightHand && handType != CarryPointType.RightElbow) return;

        bool isLeftArm = handType == CarryPointType.LeftHand || handType == CarryPointType.LeftElbow;
        int layerIndex = Animator.GetLayerIndex((isLeftArm ? "LeftArm" : "RightArm"));
        if(layerIndex < 0 ) return;
        float currentLayerWeight = Animator.GetLayerWeight(layerIndex);
        if((putItUp && currentLayerWeight > .95f) || !putItUp && currentLayerWeight < .05f) return;
        Animator.SetLayerWeight(layerIndex, (putItUp ? 1 : 0));
    }

    public virtual void ChangeArmsState(CarryPointType pointType, bool putItUp)
    {
        if (pointType != CarryPointType.Hands && pointType != CarryPointType.Elbows) return;
;
        int layerIndex = Animator.GetLayerIndex("Arms");
        if (layerIndex < 0) return;
        float currentLayerWeight = Animator.GetLayerWeight(layerIndex);
        if ((putItUp && currentLayerWeight > .95f) || !putItUp && currentLayerWeight < .05f) return;
        Animator.SetLayerWeight(layerIndex, (putItUp ? 1 : 0));
    }

    public virtual bool GetArmState(CarryPointType handType)
    {
        if( handType != CarryPointType.LeftHand && handType != CarryPointType.LeftElbow && 
            handType != CarryPointType.RightHand && handType != CarryPointType.RightElbow) return false;
        
        bool isLeftArm = handType == CarryPointType.LeftHand || handType == CarryPointType.LeftElbow;
        int layerIndex = Animator.GetLayerIndex((isLeftArm ? "LeftArm" : "RightArm"));
        float currentLayerWeight = Animator.GetLayerWeight(layerIndex);
        bool handState = currentLayerWeight > .95f;
        return handState;
    }

    #endregion
    
    
    #region Cashier

    public virtual void ChangeArmsState(bool putItUp)
    {
        if (putItUp && IsArmsUp() || !putItUp && !IsArmsUp()) return;
        int layerIndex = Animator.GetLayerIndex("UpperBody");
        if(layerIndex < 0 ) return;
        Animator.SetLayerWeight(layerIndex, (putItUp ? 1 : 0));
    }

    protected virtual bool IsArmsUp()
    {
        int layerIndex = Animator.GetLayerIndex("UpperBody");
        float currentLayerWeight = Animator.GetLayerWeight(layerIndex);
        bool isArmsUp = currentLayerWeight > .95f;
        return isArmsUp;
    }

    #endregion
}
