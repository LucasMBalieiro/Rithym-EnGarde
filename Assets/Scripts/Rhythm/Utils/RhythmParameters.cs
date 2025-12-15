using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Rhythm.Utils
{
    [System.Serializable]
    public class RhythmParameters
    {
        [Header("Music")]
        public AudioMixerGroup audioMixerGroup;
        public AudioSource trackPrefab;
        public List<SoundData> soundsToPlay;

        [Header("Beat Rhythm")] 
        public int bpm;
        public float errorMarginMs;

        [Header("Input")] 
        public float inputCooldown;
    }
}
