using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Utils
{
   [System.Serializable]
   public class CombatParameters
   {
      [MinMaxSlider(0f, 5f)] [Tooltip("Min is input cooldown, Max is combo tolerance")] 
      public Vector2 attackInterval;
      
      public int maxHitBuffer;
   }
}
