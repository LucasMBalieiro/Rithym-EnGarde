using System.Collections;
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
        private CameraController cameraController;
        
        [Header("Movement")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float runSpeed;
        [SerializeField] private float runAcceleration;
        [SerializeField] private float drag;
        [SerializeField] private float airAcceleration;
        [SerializeField] private float airDrag;
        
        [SerializeField] private float jumpHeight;
        [SerializeField] private float gravity;
        private bool canDoubleJump = true;
        
        [Header("Crosshair")]
        [SerializeField] private RawImage targetCrosshairImage;
        
        private float verticalVelocity;
        private float antiBump;
        private bool jumpedLastFrame;
        private float stepOffset;
        private PlayerMovementState lastMovementState = PlayerMovementState.Falling;
        
        private Vector2 cameraRotation = Vector2.zero;
        private Vector2 playerTargetRotation = Vector2.zero;

        private PlayerMoveInputs playerMoveInputs;
        private PlayerActionInputs playerActionInputs;
        private CharacterController characterController;
        private PlayerState playerState;
        
        private PlayerDash playerDash;
        [HideInInspector] public bool dashing = false;
        
        private PlayerWallrun playerWallrun;
        [HideInInspector] public bool wallrun = false;
        [HideInInspector] public bool wallrunningBuffer = false;
        private Vector3 wallrunVelocity;
        private Vector3 lateralJumpImpulse;
        
        public UnityEvent<float> onNoteHit;
        
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerActionInputs = GetComponent<PlayerActionInputs>();
            playerMoveInputs = GetComponent<PlayerMoveInputs>();
            playerState = GetComponent<PlayerState>();
            cameraController = playerCamera.GetComponent<CameraController>();
            
            playerDash = GetComponent<PlayerDash>();
            playerWallrun = GetComponent<PlayerWallrun>();
            
            antiBump = runSpeed;
            stepOffset = characterController.stepOffset;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            Initialize();
        }

        private void Initialize()
        {
            playerDash.Initialize(playerCamera, cameraController);
            playerWallrun.Initialize(playerCamera, cameraController, groundLayer);
        }

        private void Update()
        {
            UpdateMovementState();
            HandleVerticalMovement();
            HandleLateralMovement();
            HandleWallrunMovement();
            
            HandleAttackInput();
        }

        private void LateUpdate()
        {
            HandleCameraMovement();
        }
        
        private void UpdateMovementState()
        {
            lastMovementState = playerState.CurrentPlayerMovementState;
            
            bool isMovementInput = playerMoveInputs.MovementInput != Vector2.zero;
            bool isMovingLaterally = IsMovingLaterally();
            bool isGrounded = IsGrounded();
            
            PlayerMovementState lateralState = isMovementInput || isMovingLaterally
                ? PlayerMovementState.Running
                : PlayerMovementState.Idling;
            
            playerState.SetMovementState(lateralState);

            if (isGrounded) canDoubleJump = true;
            
            if((!isGrounded || jumpedLastFrame) && characterController.velocity.y > 0f)
            {
                playerState.SetMovementState(PlayerMovementState.Jumping);
                jumpedLastFrame = false;
                characterController.stepOffset = 0f;
            }
            else if((!isGrounded || jumpedLastFrame) && characterController.velocity.y <= 0f)
            {
                playerState.SetMovementState(PlayerMovementState.Falling);
                jumpedLastFrame = false;
                characterController.stepOffset = 0f;
            }
            else
            {
                characterController.stepOffset = stepOffset;
            }
            
            if(dashing) playerState.SetMovementState(PlayerMovementState.Dashing);
            if(wallrun) playerState.SetMovementState(PlayerMovementState.Wallrunning);
        }
        
        private void HandleVerticalMovement()
        {
            if (wallrun)
            {
                verticalVelocity = 0;
                return;
            }
            
            bool isGrounded = playerState.InGroundedState();
            
            verticalVelocity -= gravity * Time.deltaTime;
            
            if (isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -antiBump;
            }
            
            Jump();

            if (playerState.IsStateGroundedState(lastMovementState) && !isGrounded)
            {
                verticalVelocity += antiBump;
            }
        }
        
        private void HandleLateralMovement()
        {
            if (dashing || wallrun) return;
            
            bool isGrounded = IsGrounded();
            float dragMagnitude = isGrounded ? drag : airDrag;
            float accelerationMagnitude = isGrounded ? runAcceleration : airAcceleration;
            
            Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * playerMoveInputs.MovementInput.x + cameraForwardXZ * playerMoveInputs.MovementInput.y;
            
            Vector3 movementDelta = movementDirection * (accelerationMagnitude * Time.deltaTime);
            Vector3 newVelocity = characterController.velocity + movementDelta;
            
            Vector3 currentDrag = newVelocity.normalized * (dragMagnitude * Time.deltaTime);
            newVelocity = (newVelocity.magnitude > dragMagnitude * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(new Vector3(newVelocity.x, 0f, newVelocity.z), runSpeed);
            newVelocity.y += verticalVelocity;
            newVelocity = !isGrounded ? HandleSteepWalls(newVelocity) : newVelocity;
            
            newVelocity += lateralJumpImpulse;
            lateralJumpImpulse = Vector3.zero; 
            
            characterController.Move(newVelocity * Time.deltaTime);
        }
        
        private void HandleWallrunMovement()
        {
            if (!wallrun) return;

            characterController.Move(playerWallrun.GetWallRunVelocity() * Time.deltaTime);
        }

        private void Jump()
        {
            if (playerMoveInputs.JumpPressed)
            {
                if (playerState.InGroundedState())
                {
                    jumpedLastFrame = true;
                    verticalVelocity += Mathf.Sqrt(jumpHeight * 3 * gravity);
                }
                else if(!wallrunningBuffer && canDoubleJump && !wallrunningBuffer)
                {
                    verticalVelocity = 0;
                    canDoubleJump = false;
                    jumpedLastFrame = true;
                    verticalVelocity += Mathf.Sqrt(jumpHeight * 3 * gravity);
                }
            }
        }
        
        public void WallJump(Vector3 jumpDirection)
        {
            verticalVelocity = jumpDirection.y;
            lateralJumpImpulse = new Vector3(jumpDirection.x, 0, jumpDirection.z);
        }
        
        private Vector3 HandleSteepWalls(Vector3 velocity)
        {
            Vector3 normal = CharacterControllerUtils.GetNormalWithSphereCast(characterController, groundLayer);
            
            float angle = Vector3.Angle(normal, Vector3.up);
            bool validAngle = angle <= characterController.slopeLimit;

            if (!validAngle && verticalVelocity < 0f) velocity = Vector3.ProjectOnPlane(velocity, normal);
            
            return velocity;
        }
        
        private void HandleCameraMovement()
        {
            cameraRotation.x += lookSensitivityX * playerMoveInputs.LookInput.x;
            cameraRotation.y = Mathf.Clamp(cameraRotation.y - lookSensitivityY * playerMoveInputs.LookInput.y, -89, 89);
            transform.localRotation = Quaternion.Euler(0f, cameraRotation.x, 0f);
    
            float currentTilt = playerCamera.transform.localEulerAngles.z;
            playerCamera.transform.localRotation = Quaternion.Euler(cameraRotation.y, 0, currentTilt);
        }

        private void HandleAttackInput()
        {
            if (playerActionInputs.AttackPressed)
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
                Debug.Log("Perfect");
            }
            else
            {
                accuracyColor = Color.red;
                Debug.Log("Half hit");
            }

            targetCrosshairImage.color = accuracyColor;
            targetCrosshairImage.DOColor(Color.white, 0.2f);
        }

        private bool IsMovingLaterally()
        {
            Vector3 lateralVelocity = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z);

            return lateralVelocity.magnitude > 0.01f;
        }

        public bool IsGrounded()
        {
            return playerState.InGroundedState() ? IsGroundedWhileGrounded() : IsGroundedWhileAirBorne();
        }

        private bool IsGroundedWhileGrounded()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - (characterController.height/2), transform.position.z);
            
            return Physics.CheckSphere(spherePosition, characterController.radius, groundLayer, QueryTriggerInteraction.Ignore);
        }

        private bool IsGroundedWhileAirBorne()
        {
            Vector3 normal = CharacterControllerUtils.GetNormalWithSphereCast(characterController, groundLayer);
            
            float angle = Vector3.Angle(normal, Vector3.up);
            bool validAngle = angle <= characterController.slopeLimit;
            
            return characterController.isGrounded && validAngle;
        }

        public float GetVerticalVelocity()
        {
            return verticalVelocity;
        }
    }
}
