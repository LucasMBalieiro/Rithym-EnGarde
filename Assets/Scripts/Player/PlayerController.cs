using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR;

namespace Player
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float lookSensitivityX = 0.1f;
        [SerializeField] private float lookSensitivityY = 0.1f;
        [SerializeField] private float lookLimitV = 89f;
        
        [Header("Movement")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float runSpeed;
        [SerializeField] private float runAcceleration;
        [SerializeField] private float drag;
        [SerializeField] private float airAcceleration;
        [SerializeField] private float airDrag;
        
        [SerializeField] private float jumpHeight;
        [SerializeField] private float gravity;
        
        [Header("Crosshair")]
        [SerializeField] private RawImage targetCrosshairImage;
        
        private float _verticalVelocity;
        private float _antiBump;
        private bool _jumpedLastFrame;
        private float _stepOffset;
        private PlayerMovementState _lastMovementState = PlayerMovementState.Falling;
        
        
        
        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;

        private PlayerMoveInputs _playerMoveInputs;
        private PlayerActionInputs _playerActionInputs;
        private CharacterController _characterController;
        private PlayerState _playerState;
        
        public UnityEvent<float> onNoteHit;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerActionInputs = GetComponent<PlayerActionInputs>();
            _playerMoveInputs = GetComponent<PlayerMoveInputs>();
            _playerState = GetComponent<PlayerState>();
            
            _antiBump = runSpeed;
            _stepOffset = _characterController.stepOffset;
        }

        private void Update()
        {
            UpdateMovementState();
            HandleVerticalMovement();
            HandleLateralMovement();

            HandleAttackInput();
        }

        private void UpdateMovementState()
        {
            _lastMovementState = _playerState.CurrentPlayerMovementState;
            
            bool isMovementInput = _playerMoveInputs.MovementInput != Vector2.zero;
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
        
        private void HandleVerticalMovement()
        {
            bool isGrounded = _playerState.InGroundedState();
            
            _verticalVelocity -= gravity * Time.deltaTime;
            
            if (isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = -_antiBump;
            }
            
            if (_playerMoveInputs.JumpPressed && isGrounded)
            {
                _verticalVelocity += Mathf.Sqrt(jumpHeight * 3 * gravity);
                _jumpedLastFrame = true;
            }

            if (_playerState.IsStateGroundedState(_lastMovementState) && !isGrounded)
            {
                _verticalVelocity += _antiBump;
            }
        }
        
        private void HandleLateralMovement()
        {
            bool isGrounded = IsGrounded();
            float dragMagnitude = isGrounded ? drag : airDrag;
            float accelerationMagnitude = isGrounded ? runAcceleration : airAcceleration;
            
            Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerMoveInputs.MovementInput.x + cameraForwardXZ * _playerMoveInputs.MovementInput.y;
            
            Vector3 movementDelta = movementDirection * (accelerationMagnitude * Time.deltaTime);
            Vector3 newVelocity = _characterController.velocity + movementDelta;
            
            Vector3 currentDrag = newVelocity.normalized * (dragMagnitude * Time.deltaTime);
            newVelocity = (newVelocity.magnitude > dragMagnitude * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(new Vector3(newVelocity.x, 0f, newVelocity.z), runSpeed);
            newVelocity.y += _verticalVelocity;
            newVelocity = !isGrounded ? HandleSteepWalls(newVelocity) : newVelocity;
            
            _characterController.Move(newVelocity * Time.deltaTime);
        }

        private Vector3 HandleSteepWalls(Vector3 velocity)
        {
            Vector3 normal = CharacterControllerUtils.GetNormalWithSphereCast(_characterController, groundLayer);
            
            float angle = Vector3.Angle(normal, Vector3.up);
            bool validAngle = angle <= _characterController.slopeLimit;

            if (!validAngle && _verticalVelocity < 0f) velocity = Vector3.ProjectOnPlane(velocity, normal);
            
            return velocity;
        }

        private void HandleAttackInput()
        {
            if (_playerActionInputs.AttackPressed)
            {
                if (Note.HittableNote)
                {
                    Note noteToHit = Note.HittableNote;

                    noteToHit.RegisterHit();

                    float accuracy = noteToHit.GetHitAccuracy();
                    
                    FlashCrosshair(accuracy);
                    onNoteHit.Invoke(accuracy);
                }
                else
                {
                    Debug.Log("Miss");
                    onNoteHit.Invoke(0f);
                }
            }
        }
        
        private void FlashCrosshair(float accuracy)
        {
            Color accuracyColor;
            targetCrosshairImage.DOKill();

            if (accuracy >= .90)
            {
                accuracyColor = Color.yellow;
                Debug.Log("+90%");
            }
            else
            {
                accuracyColor = Color.red;
                Debug.Log("-90%");
            }

            targetCrosshairImage.color = accuracyColor;
            targetCrosshairImage.DOColor(Color.white, 0.2f);
        }
        
        private void LateUpdate()
        {
            HandleCameraMovement();
        }

        private void HandleCameraMovement()
        {
            _cameraRotation.x += lookSensitivityX * _playerMoveInputs.LookInput.x;
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSensitivityY * _playerMoveInputs.LookInput.y, -lookLimitV, lookLimitV);
            
            _playerTargetRotation.x += transform.eulerAngles.x + lookSensitivityX * _playerMoveInputs.LookInput.x;
            transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);
            
            playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);
        }

        private bool IsMovingLaterally()
        {
            Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);

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
