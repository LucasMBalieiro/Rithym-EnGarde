using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Attack.Data.Scriptables
{
    [CreateAssetMenu(fileName = "HitShape", menuName = "Combat/New HitDetection Shape")]
    public class HitDetectionScriptable : ScriptableObject
    { 
        public enum OverlapShape { None, Box, Sphere, Capsule }

        public OverlapShape shape;
        
        [Space(10)]
        [ShowIf(nameof(shape), OverlapShape.Box)]
        public Vector3 boxSize;
        [ShowIf(nameof(shape), OverlapShape.Box)]
        public Quaternion boxOrientation = Quaternion.identity;
    
        [Space(10)]
        [ShowIf(nameof(shape), OverlapShape.Sphere)]
        public int sphereRadius;
    
        [Space(10)]
        [ShowIf(nameof(shape), OverlapShape.Capsule)]
        public Vector3 centerSphereStart, centerSphereEnd;
        [ShowIf(nameof(shape), OverlapShape.Capsule)]
        public int capsuleRadius;

        
        [Space(10)]
        public bool useAlloc = true;
        
        [Layer] public int[] layers;
        public int GetLayerMask()
        {
            var layerMask = 0;
            
            if (layers.Length == 0) return layerMask;
            
            for (var i = 0; i < layers.Length; i++)
            {
                layerMask |= 1 << layers[i];
            }
            
            return layerMask;
        }
    }
}
