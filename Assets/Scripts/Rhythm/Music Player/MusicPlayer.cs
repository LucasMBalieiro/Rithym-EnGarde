using System.Collections.Generic;
using System.Linq;
using Rhythm.Storage;
using Rhythm.Utils;
using UnityEngine;
using Utils;

namespace Rhythm.Music_Player
{
    public class MusicPlayer
    {
        private AudioSource _trackPlayerPrefab;
        private Transform _audioSrcRoot;
        
        private AudioSource _mainTrack;
        private Dictionary<string, AudioSource> _audioDictionary;
        
        public MusicPlayer(RhythmParameters parameters, Transform audioSrcRoot)
        {
            _trackPlayerPrefab = parameters.trackPrefab;
            _audioSrcRoot = audioSrcRoot;
            
            _audioDictionary = new Dictionary<string, AudioSource>();
        }
        public void Update()
        {
            if (!_mainTrack)
                return;
            
            var timePosition = AudioSettings.dspTime;
            DataStorage.MainTrackTimePositionMs = timePosition * 1000d;
            // Debug.Log($"Time for {_mainTrack.name} is {timePosition}");

            // METHOD FOR STORING EVERY TRACK TIME
            // for (var i = 0; i < _audioDictionary.Count; i++)
            // {
            //     var entry = _audioDictionary.ElementAt(i);
            //
            //     var src = entry.Value;
            //     if (!src.isPlaying)
            //         continue;
            //     
            //     var timePositionMs = src.time;
            //     // Send this value to DataStorage
            //     Debug.Log($"Time for {src.name} is {timePositionMs}");
            // }
        }

        public void OnDisable()
        {
            for (var i = 0; i < _audioDictionary.Count; i++)
            {
                var entry = _audioDictionary.ElementAt(i);

                var src = entry.Value;
                src.Pause();
            }
        }

        public void AddTrack(SoundData sound, bool play = true)
        {
            if (_audioDictionary.ContainsKey(sound.sound_id))
            {
                Debug.LogWarning($"Invalid sound ID: {sound.sound_id}. This ID is already in use");
                return;
            }
            
            var audioSrc = Object.Instantiate(_trackPlayerPrefab, _audioSrcRoot);
            audioSrc.clip = sound.audioClip;
            audioSrc.loop = sound.loop;
            
            _audioDictionary.Add(sound.sound_id, audioSrc);

            if (!_mainTrack)
            {
                SetMainTrack(sound.sound_id);
                return;
            }
            
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
            Object.Destroy(_audioDictionary[soundID].gameObject);
            
            _audioDictionary.Remove(soundID);
        }

        public void SetMainTrack(string soundID)
        {
            var src = GetAudioSource(soundID);
            _mainTrack = src;

            DataStorage.MainTrackStartTime = AudioSettings.dspTime * 1000d;
            _mainTrack.Play();
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
