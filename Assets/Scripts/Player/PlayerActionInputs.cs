using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [DefaultExecutionOrder(-2)]
    public class PlayerActionInputs : MonoBehaviour, PlayerControls.IPlayerActionMapActions
    {
        public PlayerControls PlayerControls { get; private set; }
        
        public bool AttackPressed { get; private set; }

        private void LateUpdate()
        {
            AttackPressed = false;
        }

        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();
            
            PlayerControls.PlayerActionMap.Enable();
            PlayerControls.PlayerActionMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            PlayerControls.PlayerActionMap.Disable();
            PlayerControls.PlayerActionMap.RemoveCallbacks(this);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            AttackPressed = true;
            //GetComponentInChildren<SwordAnimator>().DoStab();
        }
    }
}