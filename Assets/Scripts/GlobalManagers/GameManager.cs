using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-5)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}
    
    [Header("Camera Sensitivity")] 
    [SerializeField] private float cameraSensitivityX;
    [SerializeField] private float cameraSensitivityY;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    public void GetCameraSensitivity(out float sensX, out float sensY)
    {
        sensX = cameraSensitivityX;
        sensY = cameraSensitivityY;
    }

    #region OffHand

    private bool offHandFree = true;
    public EventHandler<PickupItem> onEquip;

    public bool IsOffHandFree()
    {
        return offHandFree;
    }

    public void SetOffHandItem(PickupItem offHandItem)
    {
        onEquip?.Invoke(this, offHandItem);
    }

    #endregion
}
