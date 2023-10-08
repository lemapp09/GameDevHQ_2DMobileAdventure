using System;
using UnityEngine;

namespace LemApperson_2D_Mobile_Adventure.Managers
{

    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private AudioSource _ambientAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;
        [SerializeField] private AudioClip[] _sounds;
        
        // 0 - Ambient       1 - Tavern     2 - Click
        // 3 - Cha Ching     4 - Sword      5 - Scream
        
        private void Start() {
            Ambient(0);
        }

        public void Ambient(int ClipNumber) {
            _ambientAudioSource.Stop();
            _ambientAudioSource.clip = _sounds[ClipNumber];
            _ambientAudioSource.Play();
        }

        public void SFX(int ClipNumber) {
            _sfxAudioSource.PlayOneShot(_sounds[ClipNumber]);
        }
    }
}