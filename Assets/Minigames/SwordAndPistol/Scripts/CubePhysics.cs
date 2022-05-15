﻿using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts
{
    public class CubePhysics : MonoBehaviour
    {
        public float force = 100;
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0,0,force),ForceMode.Acceleration);
        }
    }
}
