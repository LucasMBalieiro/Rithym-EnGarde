using System;
using Player;
using UnityEngine;

public class PlayerWallrun : MonoBehaviour
{
    
    [Header("Condition Checks")]
    [SerializeField] private LayerMask wallLayerMask;
    private LayerMask groundLayerMask;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float minCharacterHeightMultiplier;
    
    [Header("Wall Running")]
    [SerializeField] private float wallrunSpeed;
    [SerializeField] private float stickToWallForce;
    
    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float wallJumpSideForce;
    [SerializeField] private float exitWallTime;
    private bool exitingWall = false;
    private float exitWallTimer;
    
    
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;
    
    private Vector3 _wallRunVelocity;

    private Camera playerCamera;
    private CameraController cameraController;
    private PlayerMoveInputs playerMoveInputs;
    private CharacterController characterController;
    private PlayerController playerController;
    private PlayerState playerState;

    public void Initialize(Camera pCamera, CameraController camController, LayerMask grdLayer)
    {
        playerCamera = pCamera;
        cameraController = camController;
        groundLayerMask = grdLayer;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerMoveInputs = GetComponent<PlayerMoveInputs>();
        playerController = GetComponent<PlayerController>();
        playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();

        if (playerController.wallrun)
            CalculateWallRunVelocity();
    }
    
    public Vector3 GetWallRunVelocity()
    {
        return _wallRunVelocity;
    }
    
    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, transform.right, out rightWallHit, wallCheckDistance, wallLayerMask);
        wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallHit, wallCheckDistance, wallLayerMask);
    }
    
    private bool IsGrounded()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - (characterController.height * minCharacterHeightMultiplier), transform.position.z);
            
        return Physics.CheckSphere(spherePosition, characterController.radius, groundLayerMask, QueryTriggerInteraction.Ignore);
    }
    
    private void StateMachine()
    {
        if ((wallLeft || wallRight) && playerMoveInputs.MovementInput.y > 0 && !IsGrounded() && !exitingWall)
        {
            if (!playerController.wallrun)
                StartWallRun();
            
            if (playerMoveInputs.JumpPressed)
                WallJump();
        }
        else if (exitingWall)
        {
            if (playerController.wallrun)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;
        }
        else
        {
            if (playerController.wallrun)
                StopWallRun();
        }
    }
    
    private void StartWallRun()
    {
        playerController.wallrun = true;
        playerController.wallrunningBuffer = true;
        cameraController.DoFov(90f, .2f);
        if(wallLeft) cameraController.DoTilt(-5f, .2f);
        if(wallRight) cameraController.DoTilt(5f, .2f);
    }

    private void CalculateWallRunVelocity()
    {
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if (Vector3.Dot(transform.forward, wallForward) < 0)
            wallForward = -wallForward;
        
        _wallRunVelocity = wallForward * wallrunSpeed;
        
        _wallRunVelocity += -wallNormal * stickToWallForce;
    }

    private void StopWallRun()
    {
        playerController.wallrun = false;
        Invoke(nameof(ResetWallRunningBuffer), .1f);
        
        cameraController.DoFov(70f, .2f);
        cameraController.DoTilt(0f, .1f);
    }
    
    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;
        
        playerController.WallJump(forceToApply);
        StopWallRun();
    }
    
    private void ResetWallRunningBuffer()
    {
        playerController.wallrunningBuffer = false;
    }
}
