using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
   public bool IsActive = true;
   
   private Camera CurrentCamera => currentCamera != null ? currentCamera : (currentCamera = CameraManager.Instance.GetCurrentCamera());
   private Camera currentCamera;

   private void Awake()
   {
      IsActive = true;
   }

   private void Update()
   {
      if ( IsActive && CurrentCamera != null)
         transform.rotation = CurrentCamera.transform.rotation;
   }
}
