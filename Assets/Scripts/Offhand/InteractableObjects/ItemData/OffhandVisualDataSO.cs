using UnityEngine;

[CreateAssetMenu(fileName = "OffhandVisualDataSO", menuName = "Offhand/VisualData")]
public class OffhandVisualDataSO : ScriptableObject
{
    public AudioClip pickupSound;
    public AudioClip throwSound;
    public AudioClip onHitSound;
    
    //TODO: aprender a usar o sistema de particulas 
    //public ParticleSystem trailParticles;
    //public ParticleSystem onHitParticles;
}
