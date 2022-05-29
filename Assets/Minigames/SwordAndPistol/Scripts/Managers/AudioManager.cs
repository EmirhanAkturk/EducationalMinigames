using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource buttonClickSound;
        [SerializeField] private AudioSource musicTheme;
        [SerializeField] private AudioSource sliceSound;
        [SerializeField] private AudioSource gunSound;
        [SerializeField] private AudioSource pinponSound;
        [SerializeField] private AudioSource basketballSound;


        private Dictionary<AudioType, AudioSource> audiosDictionary;

        private void Awake()
        {
            audiosDictionary = new Dictionary<AudioType, AudioSource>()
            {
                {AudioType.ButtonClickSound, buttonClickSound},
                {AudioType.MusicTheme, musicTheme},
                {AudioType.SliceSound, sliceSound},
                {AudioType.GunSound, gunSound},
                {AudioType.PinponSound, pinponSound},
                {AudioType.BasketballSound, basketballSound},
            };
        }

        private void OnDisable()
        {
            StopAllAudios();
        }

        public AudioSource GetAudioSource(AudioType audioType)
        {
            if (!audiosDictionary.ContainsKey(audioType))
            {
                Debug.Log($"{audioType} not in the dictionary!!");
                return null;
            }

            return audiosDictionary[audioType];
        }   
        
        
        public void PlaySound(AudioType audioType, Vector3? pos = null)
        {
            if (!audiosDictionary.ContainsKey(audioType))
            {
                Debug.Log($"{audioType} not in the dictionary!!");
                return;
            }

            var audioSource = audiosDictionary[audioType];

            if (pos != null)
            {
                audioSource.transform.position = pos.Value;
            } 
            
            audioSource.Play();
        }

        public void StopAllAudios()
        {
            foreach (var audioSourceType in audiosDictionary.Keys)
            {
                StopAudio(audioSourceType);
            }
        }
        
        public void StopAudio(AudioType audioType)
        {
            if (!audiosDictionary.ContainsKey(audioType))
            {
                Debug.Log($"{audioType} not in the dictionary!!");
                return;
            }

            var audioSource = audiosDictionary[audioType];

            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }
        
    }

    public enum AudioType
    {
        ButtonClickSound = 0,
        MusicTheme = 1,
        SliceSound = 2,
        GunSound = 3,
        PinponSound = 4,
        BasketballSound = 5,
    }
}

