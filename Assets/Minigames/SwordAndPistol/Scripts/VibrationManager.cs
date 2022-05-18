using System.Collections;
using System.Data;
using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts
{
    public class VibrationManager : Singleton<VibrationManager>
    {
        public void VibrateController(float duration, float frequency, float amplitude, OVRInput.Controller controller)
        {
            Debug.Log("Haptic played");
            StartCoroutine(VibrateSeconds(duration, frequency, amplitude, controller));
        }

        private IEnumerator VibrateSeconds(float duration, float frequency, float amplitude, OVRInput.Controller controller)
        {
            // Start vibration
            OVRInput.SetControllerVibration(frequency, amplitude, controller);
            
            // Executing the vibration for the duration seconds
            yield return new WaitForSeconds(duration);
            
            // End to vibration
            OVRInput.SetControllerVibration(0, 0, controller);
        }
    }
}
