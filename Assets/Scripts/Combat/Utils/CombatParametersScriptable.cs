using System;
using Unity.Collections;
using UnityEngine;

namespace Combat.Utils
{
    [CreateAssetMenu(fileName = "New Combat Parameters", menuName = "Parameters/Combat Parameters")]
    public class CombatParametersScriptable : ScriptableObject
    {
        [ReadOnly] public Guid ID = Guid.NewGuid(); 
        public CombatParameters Parameters;
    }
}
