using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class PlayerUIInputs : MonoBehaviour, PlayerControls.IPlayerUIMapActions
    {
        
        public PlayerControls PlayerControls { get; private set; }
        
        
        
        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();
            
            PlayerControls.PlayerUIMap.Enable();
            PlayerControls.PlayerUIMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            PlayerControls.PlayerUIMap.Disable();
            PlayerControls.PlayerUIMap.RemoveCallbacks(this);
        }

        public void OnOpenCloseUI(InputAction.CallbackContext context)
        {
            UIActions.OnOpenCloseUI.Invoke();
        }
    }
}
