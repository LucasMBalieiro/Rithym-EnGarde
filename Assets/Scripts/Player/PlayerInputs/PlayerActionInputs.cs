using Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [DefaultExecutionOrder(-2)]
    public class PlayerActionInputs : MonoBehaviour, PlayerControls.IPlayerActionMapActions
    {
        public PlayerControls PlayerControls { get; private set; }
        
        public bool AttackPressed { get; private set; }
        public bool ThrowPressed { get; private set; }
        public bool DropPressed { get; private set; }
        
        [Header("Interact Components")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float maxDistance;
        [SerializeField] private LayerMask interactLayer;

        private void LateUpdate()
        {
            AttackPressed = false;
            ThrowPressed = false;
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
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
            
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit,
                    maxDistance, interactLayer))
            {
                if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
            
            ThrowPressed = true;
        }

        public void OnDrop(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
            
            DropPressed = true;
        }
    }
}