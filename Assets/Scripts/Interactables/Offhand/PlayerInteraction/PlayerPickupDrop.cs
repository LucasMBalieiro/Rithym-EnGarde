using System;
using Interactables;
using Player;
using UnityEngine;

public class PlayerPickupDrop : MonoBehaviour
{
    [SerializeField] private PlayerActionInputs actionInputs;
    [SerializeField] private Camera playerCamera;
    
    private bool hasItem = false;
    private PickupItem itemHeld;
    
    private void Start()
    {
        GameManager.Instance.onEquip += OnItemEquip;
    }

    private void OnItemEquip(object sender, PickupItem item)
    {
        itemHeld = item;
        SetItemOnHand(item.gameObject);
    }

    private void Update()
    {
        ThrowHandler();
    }

    private void ThrowHandler()
    {
        if(!actionInputs.ThrowPressed || !hasItem) return;
        
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
