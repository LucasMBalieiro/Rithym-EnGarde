using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [DefaultExecutionOrder(-2)]
    public class PlayerMovement : MonoBehaviour, PlayerControls.IPlayerMovementMapActions
    {
        public PlayerControls PlayerControls { get; private set; }
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpPressed { get; private set; }

        private void LateUpdate()
        {
            JumpPressed = false;
        }

        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();
            
            PlayerControls.PlayerMovementMap.Enable();
            PlayerControls.PlayerMovementMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            PlayerControls.PlayerMovementMap.Disable();
            PlayerControls.PlayerMovementMap.RemoveCallbacks(this);
        }
        
        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(!context.performed) return;

            JumpPressed = true;
        }
    }
}
