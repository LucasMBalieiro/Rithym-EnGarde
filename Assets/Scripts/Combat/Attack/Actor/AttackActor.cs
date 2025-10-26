using Combat.Attack.Data.Scriptables;
using UnityEngine;

namespace Combat.Attack.Actor
{
    public class AttackActor
    {
        // Lógica da execução do ataque
        // Recebe: Ataque a ser executado
        // Defere para HitDetector para definir os objetos acertados
        // Defere para HitHandler para aplicar o ataque nos objetos acertados
        // Invoca o evento para atualizar AttackView
        // Retorna: ...

        private Transform _hitPosition;
        private HitHandler _handler;
        
        public AttackActor(Transform hitPosition)
        {
            this._hitPosition = hitPosition;
            this._handler = new HitHandler();
        }

        public void ExecuteAttack(AttackScriptable attackData)
        {
            var hits = HitDetector.GetHits(_hitPosition, CombatDataStorage.Parameters.maxHitBuffer, attackData.hitShape);

            if (hits == null || hits.Length == 0)
            {
                Debug.Log("Nothing to hit");
                return;
            }

            for (var i = 0; i < hits.Length; i++)
            {
                _handler.HandleHit(hits[i], attackData.attackAction);
            }
        }
    }
}
