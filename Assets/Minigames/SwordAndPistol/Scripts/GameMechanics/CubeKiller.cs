using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts
{
    public class CubeKiller : MonoBehaviour
    {
        /// <summary>
        /// Destroy(other.gameObject); 
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            Destroy(other.gameObject); 
        }


    }
}
