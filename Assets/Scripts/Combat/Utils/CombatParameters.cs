using System.Collections.Generic;
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

      public List<ComboState> comboStates;
   }

   [System.Serializable]
   public class ComboState
   {
      public int threshold;
      public string stateDescription;
      
      public Sprite frame;
      public Color thresholdColor;

      [Range(1f, 10f)] public float boostMultiplier;
   }
}
