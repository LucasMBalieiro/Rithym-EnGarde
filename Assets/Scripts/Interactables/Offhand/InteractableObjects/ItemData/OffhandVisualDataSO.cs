using UnityEngine;

[CreateAssetMenu(fileName = "OffhandVisualDataSO", menuName = "Offhand/VisualData")]
public class OffhandVisualDataSO : ScriptableObject
{
    public AudioClip pickupSound;
    public AudioClip throwSound;
    public AudioClip onHitSound;
    
    //TODO: add sistema de particulas
    //TODO: add materiais para explosoes e nuvens
}
