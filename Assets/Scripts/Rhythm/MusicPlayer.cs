using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Utils;

namespace Rhythm
{
    public class MusicPlayer : SingletonObj<MusicPlayer>
    {
        [SerializeField] private AudioSource trackPlayerPrefab;

        private Dictionary<string, AudioSource> _audioDictionary;
        
        private void Start()
        {
            _audioDictionary = new Dictionary<string, AudioSource>();
        }

        public void AddTrack(SoundData sound, bool play = true)
        {
            if (_audioDictionary.ContainsKey(sound.sound_id))
            {
                Debug.LogWarning($"Invalid sound ID: {sound.sound_id}. This ID is already in use");
                return;
            }
            
            var audioSrc = Instantiate(trackPlayerPrefab, this.transform);
            audioSrc.clip = sound.audioClip;
            audioSrc.loop = sound.loop;
            
            _audioDictionary.Add(sound.sound_id, audioSrc);
            
            if (play)
                audioSrc.Play();
        }
        public void RemoveTrack(string soundID)
        {
            if (!_audioDictionary.ContainsKey(soundID))
            {
                Debug.LogWarning($"The ID {soundID} is not being used");
                return;
            }
            
            PauseTrack(soundID);
            Destroy(_audioDictionary[soundID].gameObject);
            
            _audioDictionary.Remove(soundID);
        }


        public AudioSource GetAudioSource(string soundID)
        {
            if (_audioDictionary.TryGetValue(soundID, out var src))
                return src;

            Debug.LogWarning($"ID {soundID} not found.");
            return null;
        }
        public void PlayTrack(string soundID)
        {
            var src = GetAudioSource(soundID);
            src?.Play();
        }
        public void PauseTrack(string soundID)
        {
            var src = GetAudioSource(soundID);
            src.Pause();
        }

        public void SetLoop(string soundID, bool loopTrue)
        {
            var src = GetAudioSource(soundID);
            src.loop = loopTrue;
        }
    }
}
