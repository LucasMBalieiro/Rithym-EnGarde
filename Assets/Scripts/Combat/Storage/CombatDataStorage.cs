using System.Linq;
using Combat.Attack.Data;
using Combat.Utils;
using UnityEngine;

public static class CombatDataStorage
{
    // General Parameters
    public static CombatParameters Parameters { get; private set; }
    
    // Sequence Data
    public static AttackSequence AtkSequence { get; set; }
    
    // Attack Parameters
    public static bool AttackIsOnBeat { get; set; }

    public static Transform Target;
    
    // Combo Parameters
    public static int Combo;
    public static ComboState CurrentComboState;
    public static float BoostMultiplier => CurrentComboState.boostMultiplier;
    
    public static void InitializeStorage(CombatParameters parameters)
    {
        Parameters = parameters;
        
        AttackIsOnBeat = false;
        SetComboCounter(0);
    }

    public static void Cleanup()
    {
        AtkSequence = null;
        AttackIsOnBeat = false;
    }

    public static void SetComboCounter(int counter)
    {
        Combo = counter;
        
        for (var i = 0; i < Parameters.comboStates.Count; i++)
        {
            if (Combo >= Parameters.comboStates[i].threshold) continue;
            
            CurrentComboState = Parameters.comboStates[i];
            return;
        }
    }
}
