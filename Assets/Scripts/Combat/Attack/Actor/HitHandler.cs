using Combat.Attack.Data.Scriptables;
using Combat.Hittables;
using UnityEngine;

namespace Combat.Attack.Actor
{
    public class HitHandler
    {
        // Aplicação de efeitos on-hit
        // Recebe: Lista de objetos que foram acertados pelo ataque
        // Itera pela lista e aplica os devidos efeitos on-hit de cada um (se aplicável)
        // Retorna: ...

        public void HandleHit(Collider target, ActionScriptable actionData)
        {
            var hittable = target.GetComponent<IHitEffect>();
            if (hittable == null)
            {
                Debug.LogWarning($"Target {target.gameObject.name} does not have a HitEffect interface");
                return;
            }
            
            hittable.ApplyHitEffect(actionData.attackDamage * CombatDataStorage.CurrentComboState.boostMultiplier);
        }

        public void HandleContact(Vector3 contactPoint, Collider entity)
        {
            var hittable = entity.GetComponent<IHitEffect>();
            if (hittable == null)
            {
                Debug.LogWarning($"Target {entity.gameObject.name} does not have a HitEffect interface");
                return;
            }
            
            hittable.ApplyContactEffect(contactPoint);
        }
    }
}
