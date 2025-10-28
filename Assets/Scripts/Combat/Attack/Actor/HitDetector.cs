using System;
using Combat.Attack.Data.Scriptables;
using UnityEngine;

namespace Combat.Attack.Actor
{
    public static class HitDetector
    {
        // Detecção de hits
        // Recebe: Parâmetros de HitDetection, centro do ataque
        // Gera um overlap shape na posição desejada
        // Retorna: A lista de objetos detectados
        public static Collider[] GetHits(Transform detectionPosition, HitDetectionScriptable hitscanData)
        {
            Collider[] hits;
            switch (hitscanData.shape)
            {
                case HitDetectionScriptable.OverlapShape.Box when hitscanData.useAlloc:
                    hits = BoxDetection(detectionPosition, hitscanData);
                    break;
                case HitDetectionScriptable.OverlapShape.Box when !hitscanData.useAlloc:
                    BoxDetectionNonAlloc(detectionPosition, hitscanData, out hits);
                    break;
                case HitDetectionScriptable.OverlapShape.Sphere when hitscanData.useAlloc:
                    hits = SphereDetection(detectionPosition, hitscanData);
                    break;
                case HitDetectionScriptable.OverlapShape.Sphere when !hitscanData.useAlloc:
                    SphereDetectionNonAlloc(detectionPosition, hitscanData, out hits);
                    break;
                case HitDetectionScriptable.OverlapShape.Capsule when hitscanData.useAlloc:
                    hits = CapsuleDetection(detectionPosition, hitscanData);
                    break;
                case HitDetectionScriptable.OverlapShape.Capsule when !hitscanData.useAlloc:
                    CapsuleDetectionNonAlloc(detectionPosition, hitscanData, out hits);
                    break;
                case HitDetectionScriptable.OverlapShape.None:
                default:
                    hits = Array.Empty<Collider>();
                    break;
            }
            
            return hits;
        }

        private static Collider[] BoxDetection(Transform detectionPosition, HitDetectionScriptable hitscanData)
        {
            var hits = Physics.OverlapBox(
                detectionPosition.position,
                hitscanData.boxSize, 
                hitscanData.boxOrientation, 
                hitscanData.GetLayerMask());
            return hits;
        }
        private static void BoxDetectionNonAlloc(Transform detectionPosition, HitDetectionScriptable hitscanData, out Collider[] results)
        {
            results = new Collider[] {};
            var count = Physics.OverlapBoxNonAlloc(
                detectionPosition.position,
                hitscanData.boxSize,
                results,
                hitscanData.boxOrientation, 
                hitscanData.GetLayerMask());
        }
        
        private static Collider[] SphereDetection(Transform detectionPosition, HitDetectionScriptable hitscanData)
        {
            var hits = Physics.OverlapSphere(
                detectionPosition.position,
                hitscanData.sphereRadius, 
                hitscanData.GetLayerMask());
            return hits;
        }
        private static void SphereDetectionNonAlloc(Transform detectionPosition, HitDetectionScriptable hitscanData, out Collider[] results)
        {
            results = new Collider[] {};
            var count = Physics.OverlapSphereNonAlloc(
                detectionPosition.position,
                hitscanData.sphereRadius,
                results,
                hitscanData.GetLayerMask());
        }
        
        private static Collider[] CapsuleDetection(Transform detectionPosition, HitDetectionScriptable hitscanData)
        {
            var hits = Physics.OverlapCapsule(
                detectionPosition.position + hitscanData.centerSphereStart,
                detectionPosition.position + hitscanData.centerSphereEnd,
                hitscanData.capsuleRadius, 
                hitscanData.GetLayerMask());
            return hits;
        }
        private static void CapsuleDetectionNonAlloc(Transform detectionPosition, HitDetectionScriptable hitscanData, out Collider[] results)
        {
            results = new Collider[] {};
            var count = Physics.OverlapCapsuleNonAlloc(
                detectionPosition.position + hitscanData.centerSphereStart,
                detectionPosition.position + hitscanData.centerSphereEnd,
                hitscanData.capsuleRadius,
                results,
                hitscanData.GetLayerMask());
        }

        public static (Vector3 point, Collider pointEntity) GetRaycastPoint(Transform detectionPosition, HitDetectionScriptable hitscanData)
        {
            var hit = DoRaycast(detectionPosition.position, 2, hitscanData.GetLayerMask());

            if (!hit.HasValue)
            {
                return (Vector3.zero, null);
            }

            return (hit.Value.point, hit.Value.collider);
        }

        private static RaycastHit? DoRaycast(Vector3 pos, float distance, int layer)
        {
            RaycastHit hit;
            var isHit = Physics.Raycast(pos, Vector3.forward, out hit, 2, layer);

            return isHit ? hit : null;
        }
    }
}
