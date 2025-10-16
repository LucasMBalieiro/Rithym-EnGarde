using System;
using Player;
using Rhythm._Referee;
using Rhythm.Beat_Metronome;
using Rhythm.Storage;
using Rhythm.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = System.Object;

namespace Rhythm.Rhythm_Judge
{
    public class RhythmJudge : PlayerControls.IPlayerActionMapActions
    {
        public PlayerControls PlayerControls { get; private set; }

        private bool _inputTriggered;
        private float _inputCooldown;
        
        public RhythmJudge(RhythmParameters parameters)
        {
            _inputTriggered = false;
            _inputCooldown = parameters.inputCooldown;
            
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();
            
            PlayerControls.PlayerActionMap.Enable();
            PlayerControls.PlayerActionMap.SetCallbacks(this);

            BeatManager.BeatExit += ResetAction;
        }
        
        public void OnDisable()
        {
            PlayerControls.PlayerActionMap.Disable();
            PlayerControls.PlayerActionMap.RemoveCallbacks(this);
        }
        
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            if (_inputTriggered)
                return;

            _inputTriggered = true;
            
            var onBeat = DataStorage.ActiveBeat;
            BeatManager.CallAttack(onBeat);
        }
        private void ResetAction() => _inputTriggered = false;
    }
}
