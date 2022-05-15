using System;
using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts
{
    public class CompositorLayerLoadingScreen : MonoBehaviour
    {
        [SerializeField] private OVROverlay overlayBackground;
        [SerializeField] private OVROverlay overlayText;

        private OVRCameraRig OVRCameraRig
        {
            get
            {
                if (ovrCameraRig == null || ovrCameraRig.Equals(null))
                {
                    ovrCameraRig = FindObjectOfType<OVRCameraRig>();
                }

                return ovrCameraRig;
            }
        }
        
        private OVRCameraRig ovrCameraRig;
        
        private void Awake()
        {
            overlayBackground.enabled = false;
            overlayText.enabled = false;
        }

        private void OnEnable()
        {
            ShowOVROverlay();
            Debug.Log("On enable");
        }

        private void OnDisable()
        {
            Debug.Log("On disable");
            HideOVROverlay();
        }

        private void ShowOVROverlay()
        {
            var centerEyeAnchorPos = OVRCameraRig.centerEyeAnchor.position;

            overlayText.transform.position = centerEyeAnchorPos + Vector3.forward * 3f;
            
            overlayBackground.enabled = true;
            overlayText.enabled = true;
        }

        private void HideOVROverlay()
        {
            overlayBackground.enabled = false;
            overlayText.enabled = false;
        }
    }
}
