using System.Collections.Generic;
using Combat.Attack._Manager;
using Combat.Attack.Data;
using Combat.Attack.Data.Scriptables;
using Combat.Utils;
using Combo;
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
        private ComboManager _comboManager;

        // Control variables
        private bool _inputTriggered;
        private bool _comboTimer;

        protected override void Awake()
        {
            base.Awake();
            
            CombatDataStorage.InitializeStorage(parameters.parameters);
            
            // Rude initialization, change later
            CombatDataStorage.AtkSequence = new AttackSequence(attacks.Count, attacks.ToArray());
        }

        private void Start()
        {
            // Initialize managers
            _attackManager = new AttackManager();
            _comboManager = new ComboManager();
            
            EnableInputMap();
        }

        private void OnDisable()
        {
            DisableInputMap();
        
            // Disable managers
            _attackManager?.OnDisable();
            _comboManager?.OnDisable();
        
            CombatDataStorage.Cleanup();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            if (_inputTriggered)
                return;

            _inputTriggered = true;
            if (_comboTimer)
                CancelInvoke(nameof(ResetCombo));
        
            var onBeat = BeatManager.Instance.CheckOnBeat();
            CombatDataStorage.AttackIsOnBeat = onBeat;
            
            _attackManager.HandleAttack();
            
            _comboManager.UpdateComboCounter();
            _comboTimer = true;
            Invoke(nameof(ResetCombo), CombatDataStorage.Parameters.attackInterval.y);
            
            _inputTriggered = false;
        }

        public void OnInteract(InputAction.CallbackContext context) { return; }
        public void OnThrow(InputAction.CallbackContext context) { return; }
        public void OnDrop(InputAction.CallbackContext context) { return; }

        
        public void ResetCombo()
        {
            _comboManager.ResetComboCounter();
            _comboTimer = false;
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