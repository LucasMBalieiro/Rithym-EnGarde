using UnityEngine;

namespace Player
{
    public static class CharacterControllerUtils
    {
        public static Vector3 GetNormalWithSphereCast(CharacterController characterController, LayerMask layerMask)
        {
            Vector3 normal = Vector3.up;
            Vector3 center = characterController.transform.position + characterController.center;
            float distance = characterController.height / 2f + characterController.stepOffset + 0.01f;

            RaycastHit hit;
            if (Physics.SphereCast(center, characterController.radius, Vector3.down, out hit, distance, layerMask))
            {
                normal = hit.normal;
            }

            return normal;
        }
    }
}
