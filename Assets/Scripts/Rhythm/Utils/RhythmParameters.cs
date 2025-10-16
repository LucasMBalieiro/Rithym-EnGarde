using System.Collections.Generic;
using UnityEngine;

namespace Rhythm.Utils
{
    [System.Serializable]
    public class RhythmParameters
    {
        [Header("Music")]
        public AudioSource trackPrefab;
        public List<SoundData> soundsToPlay;

        [Header("Beat Rhythm")] 
        public int bpm;
        public float errorMarginMs;

        [Header("Input")] 
        public float inputCooldown;
    }
}
