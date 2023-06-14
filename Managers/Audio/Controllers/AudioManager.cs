using MergeIdle.Scripts.Configs.Audio.Data;
using MergeIdle.Scripts.Configs.Audio.Enums;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Audio.Controllers
{
    public class AudioManager : MonoBehaviour 
    {
#pragma warning disable 649
        [SerializeField] private AudioSource _background;
        [SerializeField] private AudioSource _effect;
        [SerializeField] private AudioSource _ui;
#pragma warning restore 649

        public void UnPauseMusic()
        {
            UnPauseAudioSource(_background);
        }
    
        public void PauseMusic()
        {
            PauseAudioSource(_background);
        }

        public void UnPauseSound()
        {
            UnMuteAudioSource(_effect);
        }
    
        public void PauseSound()
        {
            MuteAudioSource(_effect);
        }
        
        public void PlayBackground(EAudio audioType)
        {
            AudioItem audio = Utils.GetAudio(audioType);

            _background.clip = audio.AudioClip;
            _background.loop = true;

            if(!_background.isPlaying) _background.Play();
        }
    
        public void PlayEffect(EAudio audioType)
        {
            AudioItem audio = Utils.GetAudio(audioType);

            if (_effect.isPlaying)
            {
                _effect.Stop();
            }

            _effect.clip = audio.AudioClip;
            _effect.loop = false;

            _effect.Play();
        }
        
        private void PauseAudioSource(AudioSource audioSource)
        {
            if(audioSource.isPlaying) audioSource.Pause();
        }
    
        private void StopAudioSource(AudioSource audioSource)
        {
            if(audioSource.isPlaying) audioSource.Stop();
        }

        private void UnPauseAudioSource(AudioSource audioSource)
        {
            if(!audioSource.isPlaying) audioSource.UnPause();
        }
        
        private void MuteAudioSource(AudioSource audioSource)
        {
            audioSource.mute = true;
        }

        private void UnMuteAudioSource(AudioSource audioSource)
        {
            audioSource.mute = false;
        }
    }
}