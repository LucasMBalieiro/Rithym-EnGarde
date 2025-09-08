using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

namespace Player
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private Camera playerCamera;
        
        [Header("Movement")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float runAcceleration = 0.25f;
        [SerializeField] private float runSpeed = 4f;
        [SerializeField] private float drag = 0.1f;
        
        [SerializeField] private float jumpHeight = 1f;
        [SerializeField] private float gravity = 35f;
        
        private float _verticalVelocity;
        private float _antiBump;
        private bool _jumpedLastFrame;
        private float _stepOffset;
        private PlayerMovementState _lastMovementState = PlayerMovementState.Falling;
        
        
        [Header("Camera")]
        [SerializeField] private float lookSensitivityX = 0.1f;
        [SerializeField] private float lookSensitivityY = 0.1f;
        [SerializeField] private float lookLimitV = 89f;
        
        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;

        private PlayerMovement _playerMovement;
        private CharacterController _characterController;
        private PlayerState _playerState;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerState = GetComponent<PlayerState>();
            
            _antiBump = runSpeed;
            _stepOffset = _characterController.stepOffset;
        }

        private void Update()
        {
            UpdateMovementState();

            HandleVerticalMovement();
            HandleLateralMovement();
        }

        private void HandleLateralMovement()
        {
            bool isGrounded = IsGrounded();
            
            Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerMovement.MovementInput.x + cameraForwardXZ * _playerMovement.MovementInput.y;
            
            Vector3 movementDelta = movementDirection * (runAcceleration * Time.deltaTime);
            Vector3 newVelocity = _characterController.velocity + movementDelta;
            
            Vector3 currentDrag = newVelocity.normalized * (drag * Time.deltaTime);
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(new Vector3(newVelocity.x, 0f, newVelocity.z), runSpeed);
            newVelocity.y += _verticalVelocity;
            newVelocity = !isGrounded ? HandleSteepWalls(newVelocity) : newVelocity;
            
            _characterController.Move(newVelocity * Time.deltaTime);
        }

        private void HandleVerticalMovement()
        {
            bool isGrounded = _playerState.InGroundedState();
            
            _verticalVelocity -= gravity * Time.deltaTime;
            
            if (isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = -_antiBump;
            }
            
            if (_playerMovement.JumpPressed && isGrounded)
            {
                _verticalVelocity += Mathf.Sqrt(jumpHeight * 3 * gravity);
                _jumpedLastFrame = true;
            }

            if (_playerState.IsStateGroundedState(_lastMovementState) && !isGrounded)
            {
                _verticalVelocity += _antiBump;
            }
        }

        private Vector3 HandleSteepWalls(Vector3 velocity)
        {
            Vector3 normal = CharacterControllerUtils.GetNormalWithSphereCast(_characterController, groundLayer);
            
            float angle = Vector3.Angle(normal, Vector3.up);
            bool validAngle = angle <= _characterController.slopeLimit;

            if (!validAngle && _verticalVelocity < 0f) velocity = Vector3.ProjectOnPlane(velocity, normal);
            
            return velocity;
        }

        private void UpdateMovementState()
        {
            _lastMovementState = _playerState.CurrentPlayerMovementState;
            
            bool isMovementInput = _playerMovement.MovementInput != Vector2.zero;
            bool isMovingLaterally = IsMovingLaterally();
            bool isGrounded = IsGrounded();
            
            PlayerMovementState lateralState = isMovementInput || isMovingLaterally
                ? PlayerMovementState.Running
                : PlayerMovementState.Idling;
            
            _playerState.SetMovementState(lateralState);

            if ((!isGrounded || _jumpedLastFrame) && _characterController.velocity.y > 0f)
            {
                _playerState.SetMovementState(PlayerMovementState.Jumping);
                _jumpedLastFrame = false;
                _characterController.stepOffset = 0f;
            }
            else if((!isGrounded || _jumpedLastFrame) && _characterController.velocity.y <= 0f)
            {
                _playerState.SetMovementState(PlayerMovementState.Falling);
                _jumpedLastFrame = false;
                _characterController.stepOffset = 0f;
            }
            else
            {
                _characterController.stepOffset = _stepOffset;
            }
        }
        
        private void LateUpdate()
        {
            HandleCameraMovement();
        }

        private void HandleCameraMovement()
        {
            _cameraRotation.x += lookSensitivityX * _playerMovement.LookInput.x;
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSensitivityY * _playerMovement.LookInput.y, -lookLimitV, lookLimitV);
            
            _playerTargetRotation.x += transform.eulerAngles.x + lookSensitivityX * _playerMovement.LookInput.x;
            transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);
            
            playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
        }

        private bool IsMovingLaterally()
        {
            Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.y);

            return lateralVelocity.magnitude > 0.01f;
        }

        private bool IsGrounded()
        {
            return _playerState.InGroundedState() ? IsGroundedWhileGrounded() : IsGroundedWhileAirBorne();
        }

        private bool IsGroundedWhileGrounded()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - (_characterController.height/2), transform.position.z);
            
            return Physics.CheckSphere(spherePosition, _characterController.radius, groundLayer, QueryTriggerInteraction.Ignore);
        }

        private bool IsGroundedWhileAirBorne()
        {
            Vector3 normal = CharacterControllerUtils.GetNormalWithSphereCast(_characterController, groundLayer);
            
            float angle = Vector3.Angle(normal, Vector3.up);
            bool validAngle = angle <= _characterController.slopeLimit;
            
            return _characterController.isGrounded && validAngle;
        }
    }
}
