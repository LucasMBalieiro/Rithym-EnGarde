using System;
using NaughtyAttributes;
using UnityEngine;

namespace Combat.Attack.Data.Scriptables
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Combat/New Attack")]
    public class AttackScriptable : ScriptableObject
    {
        [ReadOnly] public Guid ID = Guid.NewGuid();
    
        public ActionScriptable attackAction;
        public HitDetectionScriptable hitShape;
    }
}
