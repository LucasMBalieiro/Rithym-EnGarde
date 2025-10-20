using UnityEngine;

namespace Combat.Attack.Data.Scriptables
{
    [CreateAssetMenu(fileName = "Action", menuName = "Combat/New Attack Action")]
    public class ActionScriptable : ScriptableObject
    {
        public float attackDamage;
        public Animation attackAnimation;
        public AudioClip attackSoundEffect;
    }
}
