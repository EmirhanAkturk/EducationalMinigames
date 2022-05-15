using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts
{
    public class AudioManager : MonoBehaviour
    {

        public AudioSource sliceSound;
        public AudioSource gunSound;
        public AudioSource musicTheme;
        public AudioSource buttonClickSound;


        public static AudioManager instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;
        }

   
    }
}
