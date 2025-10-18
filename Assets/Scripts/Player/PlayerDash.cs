using System.Collections;
using Player;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    
    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private bool canDash = true;
    private bool dashCooldownTimer = true;
    
    private Camera playerCamera;
    private CameraController cameraController;
    private PlayerMoveInputs playerMoveInputs;
    private CharacterController characterController;
    private PlayerController playerController;
    private PlayerState playerState;

    public void Initialize(Camera pCamera, CameraController camController, PlayerState pState)
    {
        playerCamera = pCamera;
        cameraController = camController;
        playerState = pState;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerMoveInputs = GetComponent<PlayerMoveInputs>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        HandleDashInput();
        ResetDashCondition();
    }

    private void ResetDashCondition()
    {
        if (playerController.IsGrounded() && !canDash)
        {
            canDash = true;
        }
    }
    
    private void HandleDashInput()
    {
        if (playerMoveInputs.DashPressed && canDash && dashCooldownTimer && playerMoveInputs.MovementInput != Vector2.zero && playerState.CurrentPlayerMovementState != PlayerMovementState.Wallrunning)
        {
            StartCoroutine(Dash(playerMoveInputs.MovementInput));
        }
    }

    private IEnumerator Dash(Vector2 playerInput)
    {
        Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
        Vector3 dashDirection = (cameraRightXZ * playerInput.x + cameraForwardXZ * playerInput.y).normalized;

        canDash = false;
        dashCooldownTimer = false;
        playerController.dashing = true;
        cameraController.DoFov(70, dashDuration/2);
            
        float startTime = Time.time;
            
        while (Time.time < startTime + dashDuration)
        {
            Vector3 dashMovement = dashDirection * dashSpeed;
            dashMovement.y = playerController.GetVerticalVelocity();
        
            characterController.Move(dashMovement * Time.deltaTime);
                
            yield return null;
        }
        cameraController.DoFov(60, dashDuration/2);
        playerController.dashing = false;
            
        yield return new WaitForSeconds(dashCooldown);
        dashCooldownTimer = true;
    }
}
