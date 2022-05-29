using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SynchronizedXrGrab : XRGrabInteractable
{
   private NetworkingGrabbing networkingGrabbing;

   protected override void OnSelectEntered(SelectEnterEventArgs args)
   {
      if (TryGetComponent(out networkingGrabbing))
      {
         if(networkingGrabbing.IsBeingHeld) return;
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
