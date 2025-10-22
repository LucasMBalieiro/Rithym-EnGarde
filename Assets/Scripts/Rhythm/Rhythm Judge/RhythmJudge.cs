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
        // TODO TIRAR LÓGICA DE INPUT DAQUI, MOVER PARA CLASSE RESPONSÁVEL PRÓPRIA
        
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

        public bool IsOnBeat()
        {
            var onBeat = DataStorage.ActiveBeat;
            BeatManager.CallInputEvent(onBeat);
            return onBeat;
        }
        
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            if (_inputTriggered)
                return;

            _inputTriggered = true;
            
            var onBeat = DataStorage.ActiveBeat;
            BeatManager.CallInputEvent(onBeat);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            //é... realmente tem que tirar daqui esse bglh
            return; 
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            return;
        }

        public void OnDrop(InputAction.CallbackContext context)
        {
            return;
        }

        private void ResetAction() => _inputTriggered = false;
    }
}
