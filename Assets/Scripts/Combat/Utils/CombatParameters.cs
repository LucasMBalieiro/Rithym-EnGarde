using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Utils
{
   [System.Serializable]
   public class CombatParameters
   { 
      public float baseAttackRange;
      public float baseAttackDelay;
      public float baseAttackSpeed;
      public float baseAttackDamage;
      public LayerMask attackLayer;

      public float sequenceInterval;
      
      public int maxHitBuffer;
   }
}
