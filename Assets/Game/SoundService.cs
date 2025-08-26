using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class SoundService:MonoBehaviour
    {
        [SerializeField]
        private AudioSource buttonAudioSource;
        [SerializeField]
        private AudioSource spinAudioSource;
        
        [SerializeField]
        private AudioClip buttonClick;
        [SerializeField]
        private AudioClip buttonHover;
        
 


        private void Awake()
        {
            Services.Sound = this;
        }

        public void PlaySound(AudioClip clip)
        {
            
        }

        public void PlayButtonHover()
        {
            buttonAudioSource.pitch = Random.value * 0.2f + 0.9f;
            buttonAudioSource.volume = 0.35f;
            buttonAudioSource.PlayOneShot(buttonHover);
        }

        public void PlayButtonClick()
        {
            buttonAudioSource.pitch = Random.value * 0.2f + 0.9f;
            buttonAudioSource.volume = 0.35f;
            buttonAudioSource.PlayOneShot(buttonClick);
        }

        public void StartSpinning()
        {
            //spinAudioSource.loop = true;
            //spinAudioSource.Play();
        }

        public void StopSpinning()
        {
            
            //spinAudioSource.Stop();
        }
    }
}