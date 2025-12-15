using System.Collections;
using UnityEngine;

namespace Audio
{
    public class SoundEmitter : MonoBehaviour
    {
        public SoundData SoundData {private set; get;}
        
        private AudioSource audioSource;
        private Coroutine playingCoroutine;
        
        private void Awake() => audioSource = GetComponent<AudioSource>();

        public void Initialize(SoundData soundData)
        {
            SoundData = soundData;
            audioSource.clip = soundData.audioClip;
            audioSource.loop = soundData.loop;
            audioSource.outputAudioMixerGroup = soundData.audioMixerGroup;
        }

        public void Play()
        {
            if (playingCoroutine != null) StopCoroutine(playingCoroutine);
            
            audioSource.Play();
            playingCoroutine = StartCoroutine(WaitClipLenght());
        }

        public void Stop()
        {
            if (playingCoroutine != null)
            {
                StopCoroutine(playingCoroutine);
                playingCoroutine = null;
            }
            
            audioSource.Stop();
            SoundManager.Instance.ReturnSoundEmitter(this);
        }

        public void WithRandomPitch(float minPitch = -0.05f, float maxPitch = 0.05f)
        {
            audioSource.pitch += Random.Range(minPitch, maxPitch);
        }

        private IEnumerator WaitClipLenght()
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            SoundManager.Instance.ReturnSoundEmitter(this);
        }
    }
}