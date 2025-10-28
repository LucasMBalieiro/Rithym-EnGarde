using System;
using Combat.Attack.Actor;
using Combat.Attack.Data.Scriptables;
using UnityEngine;
using Combat.Attack.Judge;

namespace Combat.Attack._Manager
{
    public class AttackManager
    {
        private AttackJudge _judge;
        private AttackActor _actor;

        private AttackScriptable _currentAttack;
        private Transform _hitPosition;
        
        private float _timeSinceLastAttack;

        private bool _isActive;
        public static event Action<ActionScriptable> OnAttackView; 
        
        public AttackManager()
        {
            this._hitPosition = CombatDataStorage.Target;
            _currentAttack = null;
            
            _judge = new AttackJudge();
            _actor = new AttackActor(this._hitPosition);

            _timeSinceLastAttack = 0f;

            _isActive = true;
        }
        
        public void HandleAttack()
        {
            _currentAttack = _judge.NextAttack(CombatDataStorage.AttackIsOnBeat, _timeSinceLastAttack);
            
            if (_currentAttack == null)
                return;
            
            OnAttackView?.Invoke(_currentAttack.attackAction);
            _actor.ExecuteAttack(_currentAttack);
            
            _timeSinceLastAttack = 0f;
        }

        public void Update()
        {
            if (!_isActive)
                return;
            
            _timeSinceLastAttack += Time.deltaTime;
        }

        public void DrawGizmos()
        {
            if (!_isActive) return;
            if (_currentAttack == null) return;

            var gizmosShape = _currentAttack.hitShape;
            switch (gizmosShape.shape)
            {
                case HitDetectionScriptable.OverlapShape.Box:
                    Gizmos.DrawWireCube(_hitPosition.position, gizmosShape.boxSize);
                    break;
                case HitDetectionScriptable.OverlapShape.Sphere:
                    Gizmos.DrawWireSphere(_hitPosition.position, gizmosShape.sphereRadius);
                    break;
                case HitDetectionScriptable.OverlapShape.Capsule:
                case HitDetectionScriptable.OverlapShape.None:
                default:
                    break;
            }
        }

        public void OnDisable()
        {
            _isActive = false;
        }
    }
}
