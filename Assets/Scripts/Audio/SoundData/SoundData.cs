using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    [Serializable]
    public class SoundData
    {
        public AudioClip audioClip;
        public AudioMixerGroup audioMixerGroup;
        public bool loop;
        public bool frequentSound;
    }
}
