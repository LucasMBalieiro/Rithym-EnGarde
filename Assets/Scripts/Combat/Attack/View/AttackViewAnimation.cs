using Combat.Attack.Data.Scriptables;
using UnityEngine;
using Utils;

namespace Combat.Attack.View
{
    public class AttackViewAnimation : IViewModule<ActionScriptable>
    {
        private Animator _animator;
        
        private int GetStateHash(string state) => Animator.StringToHash(state);

        public AttackViewAnimation(Animator animator)
        {
            this._animator = animator;
        }
        
        public void ExecuteView(ActionScriptable data)
        {
            var state = GetStateHash(data.attackAnimationState);
            if (!_animator.HasState(0, state))
            {
                Debug.LogWarning($"State {data.attackAnimationState} not found. Will not execute animation");    
            }
            
            _animator.CrossFadeInFixedTime(state, .5f);
        }
    }
}
