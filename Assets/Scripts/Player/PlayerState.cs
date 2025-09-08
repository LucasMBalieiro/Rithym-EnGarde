using UnityEngine;

namespace Player
{
    public enum PlayerMovementState
    {
        Idling = 0,
        Running = 1,
        Dashing = 2,
        Jumping = 3,
        Falling = 4,
    }
    public class PlayerState : MonoBehaviour
    {
        [field: SerializeField]
        public PlayerMovementState CurrentPlayerMovementState { get; private set; } = PlayerMovementState.Idling;

        public void SetMovementState( PlayerMovementState newState )
        {
            CurrentPlayerMovementState = newState;
        }
        
        public bool InGroundedState()
        {
            return IsStateGroundedState(CurrentPlayerMovementState);
        }

        public bool IsStateGroundedState(PlayerMovementState movementState)
        {
            return movementState is PlayerMovementState.Idling or PlayerMovementState.Running or PlayerMovementState.Dashing;
        }
    }
}
