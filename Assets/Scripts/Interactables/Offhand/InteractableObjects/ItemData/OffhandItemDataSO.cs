using Offhand.InteractableObjects.ItemData;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "OffhandItemData", menuName = "Offhand/ItemData")]
public class OffhandItemDataSO : ScriptableObject
{
    public OffhandAttackType attackType;
    
    public float damage;
    public bool gravity;
    public float velocity;
    public float verticalThrowForce;
    public float mass = 1;
}
