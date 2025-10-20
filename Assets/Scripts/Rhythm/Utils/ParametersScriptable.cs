using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rhythm.Utils
{
    [CreateAssetMenu(fileName = "Rhythm Parameters", menuName = "Parameters/Rhythm Parameters")]
    public class ParametersScriptable : ScriptableObject
    {
        [ReadOnly] public Guid ID = Guid.NewGuid();
        public RhythmParameters parameters;
    }
}
