using Player;
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
            if (onBeat)
            {
                Debug.Log("Input on beat");
            }
            else
            {
                Debug.Log("Input out of beat");
            }
        }
        public void ResetAction() => _inputTriggered = false;
    }
}
