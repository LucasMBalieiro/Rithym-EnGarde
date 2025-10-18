using UnityEngine;

namespace Rhythm
{
    [System.Serializable]
    public class SoundData
    {
        public string sound_id;
        public string name;

        public AudioClip audioClip;
    
        public bool loop;
    }
}
