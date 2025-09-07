using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera playerCamera;
        
        [Header("Movement")]
        [SerializeField] private float runAcceleration = 0.25f;
        [SerializeField] private float runSpeed = 4f;
        [SerializeField] private float drag = 0.1f;
        
        [Header("Look")]
        [SerializeField] private float lookSensitivityX = 0.1f;
        [SerializeField] private float lookSensitivityY = 0.1f;
        [SerializeField] private float lookLimitV = 89f;
        
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            // Movimenta player de acordo com a posicao da camera
            Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerMovement.MovementInput.x + cameraForwardXZ * _playerMovement.MovementInput.y;
            
            // Define velocidade dado um input
            Vector3 movementDelta = movementDirection * (runAcceleration * Time.deltaTime);
            Vector3 newVelocity = characterController.velocity + movementDelta;
            
            // Adiciona atrito para parar o personagem
            Vector3 currentDrag = newVelocity * (drag * Time.deltaTime);
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(newVelocity, runSpeed);
            
            characterController.Move(newVelocity * Time.deltaTime);
        }
    }
}
