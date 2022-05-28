using System;
using Minigames.SwordAndPistol.Scripts.Managers;
using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts
{
    public class CubeExplode : MonoBehaviour
    {
        
        [SerializeField] private GameObject shatteredObject;
        [SerializeField] private GameObject mainCube;
        
        [Header("Vibration")]
        [SerializeField] private OVRInput.Controller controller = OVRInput.Controller.RTouch;
        [SerializeField] private float duration = .4f;
        [SerializeField] private float frequency = 1f;
        [SerializeField] private float amplitude = .3f;

        private bool isExplode;
        
        // Start is called before the first frame update
        void Start()
        {
            mainCube.SetActive(true);
            shatteredObject.SetActive(false);
        }

        private void OnEnable()
        {
            isExplode = false;
        }

        private void IsShot()
        {
            if(isExplode) return;

            isExplode = true;
            
            Destroy(mainCube);
            shatteredObject.SetActive(true);
            var shatterAnimation = shatteredObject.GetComponent<Animation>();
            shatterAnimation.Play();
            
            // Vibrate the Controller
            VibrationManager.Instance.VibrateController(duration, frequency, amplitude, controller);
            
            //Add score
            ScoreManager.Instance.AddScore(ScorePoints.GUNCUBE_SCOREPOINT);
            
            Destroy(shatteredObject,1);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                IsShot();
            }
        }


    }
}
