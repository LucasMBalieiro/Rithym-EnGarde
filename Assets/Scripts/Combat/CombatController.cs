using System.Collections.Generic;
using Combat.Attack._Manager;
using Combat.Attack.Data;
using Combat.Attack.Data.Scriptables;
using Combat.Utils;
using Player;
using Rhythm._Referee;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Combat
{
    public class CombatController : SingletonObj<CombatController>, PlayerControls.IPlayerActionMapActions
    {
        public PlayerControls PlayerControls { get; private set; }

        // Parameters
        [SerializeField] private CombatParametersScriptable parameters;
        [SerializeField] private List<AttackScriptable> attacks; // Temporary. Add attack sequence generation later

        [Space(20)] 
        [SerializeField] private bool attackGizmosActive;
    
        // Managers
        private AttackManager _attackManager;

        // Control variables
        private bool _inputTriggered;
        private Transform _hitPosition;
        public void UpdateHitPosition(Transform newHitPosition) => this._hitPosition = newHitPosition;
    
        private void OnEnable()
        {
            CombatDataStorage.InitializeStorage(parameters.Parameters);
        
            // Initialize managers
            _attackManager = new AttackManager(_hitPosition);
        
            EnableInputMap();
        }

        private void OnDisable()
        {
            DisableInputMap();
        
            // Disable managers
            _attackManager?.OnDisable();
        
            CombatDataStorage.Cleanup();
        }

        private void Start()
        {
            // Rude initialization, change later
            CombatDataStorage.AtkSequence = new AttackSequence(attacks.Count, attacks.ToArray());
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            if (_inputTriggered)
                return;

            _inputTriggered = true;
        
            var onBeat = BeatManager.Instance.CheckOnBeat();
            CombatDataStorage.AttackIsOnBeat = onBeat;
            _attackManager.HandleAttack();
        
            _inputTriggered = false;
        }

        private void Update()
        {
            _attackManager.Update();
        }

        private void OnDrawGizmos()
        {
            if (attackGizmosActive)
                _attackManager?.DrawGizmos();
        }

        #region InitializeInputMap
        private void EnableInputMap()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();
        
            PlayerControls.PlayerActionMap.Enable();
            PlayerControls.PlayerActionMap.SetCallbacks(this);
        }
        private void DisableInputMap()
        {
            if (PlayerControls is null)
                return;
            PlayerControls.PlayerActionMap.Disable();
            PlayerControls.PlayerActionMap.RemoveCallbacks(this);
        }
        #endregion
    }
}