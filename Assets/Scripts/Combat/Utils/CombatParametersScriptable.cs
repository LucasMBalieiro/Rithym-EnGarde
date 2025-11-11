using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Utils
{
    [CreateAssetMenu(fileName = "New Combat Parameters", menuName = "Parameters/Combat Parameters")]
    public class CombatParametersScriptable : ScriptableObject
    {
        [ReadOnly] public Guid ID = Guid.NewGuid(); 
        [FormerlySerializedAs("Parameters")] public CombatParameters parameters;

        
        
        
        [Button("Order Combo States")]
        public void OrderCombo()
        {
            parameters.comboStates = parameters.comboStates.OrderBy(s => s.threshold).ToList();
        }
    }
}
