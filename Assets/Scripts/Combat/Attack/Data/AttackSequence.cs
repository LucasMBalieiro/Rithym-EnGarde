using System.Collections.Generic;
using Combat.Attack.Data.Scriptables;

namespace Combat.Attack.Data
{
    public class AttackSequence
    {
        public List<AttackScriptable> AtkSequence;
        private int _atkCurrentStep;

        public AttackSequence(int sequenceSize, params AttackScriptable[] attacks)
        {
            AtkSequence = new List<AttackScriptable>(capacity: sequenceSize);
            AtkSequence.AddRange(attacks);
            
            _atkCurrentStep = 0;
        }

        public void AddAttackToSequence(AttackScriptable attack, int idxToInsert)
        { AtkSequence.Insert(idxToInsert, attack); }
        public void RemoveAttackFromSequence(int idxToRemove)
        { AtkSequence.RemoveAt(idxToRemove); }
        
        public AttackScriptable GetCurrentAttack() => AtkSequence[_atkCurrentStep];
        public AttackScriptable NextAttack()
        {
            _atkCurrentStep++;
            return GetCurrentAttack();
        }
        public AttackScriptable ResetAttackSequence()
        {
            _atkCurrentStep = 0;
            return GetCurrentAttack();
        } 
    }
}
