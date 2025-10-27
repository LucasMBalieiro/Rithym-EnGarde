using Combat.Attack.Data.Scriptables;
using UnityEngine;

namespace Combat.Attack.Judge
{
    public class AttackJudge
    {
        // Lógica da sequência
        // Recebe: onBeat, intervalo entre ataques
        // Avalia se deve continuar a sequência ou resetar
        // Retorna o próximo ataque a ser executado

        public AttackScriptable NextAttack(bool onBeat, float inputInterval)
        {
            var attackCooldown = CombatDataStorage.Parameters.attackInterval;
            if (inputInterval < attackCooldown.x)
                return null;
            
            var onTime = (attackCooldown.y - inputInterval) >= Mathf.Epsilon;
            var keepSequence = onBeat && onTime;

            Debug.Log($"Is on beat: {onBeat}\nLast input: {inputInterval}s ago | Is on time: {onTime}\nFinal Decision: {keepSequence}");
            
            var sequence = CombatDataStorage.AtkSequence;
            return keepSequence ? 
                sequence.NextAttack() :
                sequence.ResetAttackSequence();
        }
    }
}
