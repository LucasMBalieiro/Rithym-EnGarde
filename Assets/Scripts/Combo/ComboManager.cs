using System;

namespace Combo
{
    public class ComboManager
    {
        private int _comboCounter;

        public static event Action OnComboView; 
    
        public ComboManager()
        {
            _comboCounter = 0;
        }
    
        public void OnDisable() {}

        public void UpdateComboCounter()
        {
            var isOnBeat = CombatDataStorage.AttackIsOnBeat;

            if (isOnBeat)
                _comboCounter++;
            else
                _comboCounter = 0;
        
            // Atualizar Data
            CombatDataStorage.SetComboCounter(_comboCounter);
        
            // Atualizar View
            OnComboView?.Invoke();
        }
    }
}
