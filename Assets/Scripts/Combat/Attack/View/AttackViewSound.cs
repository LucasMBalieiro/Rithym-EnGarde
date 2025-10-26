using Combat.Attack.Data.Scriptables;
using UnityEngine;
using Utils;

namespace Combat.Attack.View
{
    public class AttackViewSound : IViewModule<ActionScriptable>
    {
        private AudioSource _audioSource;

        public AttackViewSound(AudioSource audioSource)
        {
            this._audioSource = audioSource;
        }
        
        public void ExecuteView(ActionScriptable data)
        {
            var rand = Random.Range(0, data.attackSoundEffect.Length);
            var selectedSound = data.attackSoundEffect[rand];

            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.PlayOneShot(selectedSound);
        }
    }
}
