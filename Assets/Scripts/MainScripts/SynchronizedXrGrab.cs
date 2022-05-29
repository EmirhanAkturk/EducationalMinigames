using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SynchronizedXrGrab : XRGrabInteractable
{
   private NetworkingGrabbing networkingGrabbing;

   public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
   {
      base.ProcessInteractable(updatePhase);
      if (TryGetComponent(out networkingGrabbing))
      {
         if (networkingGrabbing.IsBeingHeld)
         {
            Debug.Log("1 networkingGrabbing.IsBeingHeld : true");
            return;
         }
      }
   }

   protected override void OnSelectEntered(SelectEnterEventArgs args)
   {
      if (TryGetComponent(out networkingGrabbing))
      {
         if (networkingGrabbing.IsBeingHeld)
         {
            Debug.Log("2 networkingGrabbing.IsBeingHeld : true");
            return;
         }
      }
      
      base.OnSelectEntered(args);
   }

   // protected override void Grab()
   // {
   //    if (TryGetComponent(out networkingGrabbing))
   //    {
   //       if(networkingGrabbing.IsBeingHeld) return;
   //    }
   //    base.Grab();
   // }
}
