using Combat.Attack.Data;
using Combat.Utils;
using UnityEngine;

public static class CombatDataStorage
{
    public static CombatParameters Parameters { get; private set; }
    
    public static AttackSequence AtkSequence { get; set; }
    public static bool AttackIsOnBeat { get; set; }

    public static Transform Target;
    
    public static void InitializeStorage(CombatParameters parameters)
    {
        Parameters = parameters;
        
        AttackIsOnBeat = false;
    }

    public static void Cleanup()
    {
        AtkSequence = null;
        AttackIsOnBeat = false;
    }
}
