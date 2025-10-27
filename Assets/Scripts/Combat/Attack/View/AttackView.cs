using Combat.Attack._Manager;
using Combat.Attack.Data.Scriptables;
using UnityEngine;

namespace Combat.Attack.View
{
    public class AttackView : MonoBehaviour
    {
        [SerializeField] private Transform aimRef;

        [Header("Animation")] 
        [SerializeField] private Animator animator;
        
        [Header("Sound")] 
        [SerializeField] private AudioSource audioSource;
        
        private AttackViewAnimation _animation;
        private AttackViewSound _sound;
        private AttackViewEffect _effects;

        private void Awake()
        {
            CombatController.Instance.UpdateHitPosition(aimRef);
            
            _animation = new AttackViewAnimation(animator);
            _sound = new AttackViewSound(audioSource);
            _effects = new AttackViewEffect();
        }
    
        private void OnEnable()
        {
            AttackManager.OnAttackView += ExecuteAttackView;
        }

        private void OnDisable()
        {
            AttackManager.OnAttackView -= ExecuteAttackView;
        }

        private void ExecuteAttackView(ActionScriptable action)
        {
            _animation.ExecuteView(action);
            _sound.ExecuteView(action);
            // _effects.ExecuteView(action);
        }
    }
}
