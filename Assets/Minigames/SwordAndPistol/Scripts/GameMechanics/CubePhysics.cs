using System;
using lib.GameDepends.Enums;
using Minigames.SwordAndPistol.Scripts.Managers;
using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts
{
    public class CubePhysics : MonoBehaviour
    {
        public float force = 100;
        // Start is called before the first frame update
        void Start()
        {
            var rb = GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(0,0,force),ForceMode.Acceleration);
        }

        private void OnEnable()
        {
            GameManager.OnGameOver.AddListener(DestroyCube);
        }
        
        private void OnDisable()
        {
            GameManager.OnGameOver.RemoveListener(DestroyCube);
        }

        private void DestroyCube(MiniGameType miniGameType)
        {
            if(miniGameType != GameManager.CURRENT_MINI_GAME_TYPE) return;
            Destroy(gameObject);
        }
    }
}
