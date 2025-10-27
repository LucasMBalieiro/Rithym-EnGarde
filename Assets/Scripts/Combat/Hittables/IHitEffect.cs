using UnityEngine;

namespace Combat.Hittables
{
    public interface IHitEffect
    {
        public void ApplyHitEffect(float hitValue);     // For hits
        public void ApplyContactEffect(Vector3 point);  // For contacts (Decal, blood particles, etc.)
    }
}
