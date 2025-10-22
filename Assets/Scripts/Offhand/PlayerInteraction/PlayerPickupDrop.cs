using Player;
using UnityEngine;

public class PlayerPickupDrop : MonoBehaviour
{
    [SerializeField] private PlayerActionInputs actionInputs;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask pickupLayer;
    
    private bool hasItem = false;
    private PickupItem itemHeld;
    
    private void Update()
    {
        if (!hasItem)
        {
            InteractHandler();
        }
        else
        {
            ThrowHandler();
        }
    }

    private void InteractHandler()
    {
        if (!actionInputs.InteractPressed) return;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit,
                maxDistance, pickupLayer))
        {
            itemHeld = hit.collider.GetComponent<PickupItem>();
            SetItemOnHand(hit.collider.gameObject);
            itemHeld.Pickup();
        }
    }

    private void ThrowHandler()
    {
        if(!actionInputs.ThrowPressed) return;
        
        itemHeld.Throw(playerCamera.transform);
        
        hasItem = false;
        itemHeld = null;
    }

    private void SetItemOnHand(GameObject item)
    {
        hasItem = true;

        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.layer = LayerMask.NameToLayer("FPS ViewModel");
    }

}
