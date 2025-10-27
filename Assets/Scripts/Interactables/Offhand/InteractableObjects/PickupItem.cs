using System;
using Interactables;
using Offhand.InteractableObjects.ItemData;
using Rhythm._Referee;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private OffhandItemDataSO itemData;
    [SerializeField] private OffhandVisualDataSO visualData;
    
    [SerializeField] private GameObject explodeEffect;
    [SerializeField] private GameObject cloudEffect;
    
    private AudioSource audioSource;
    private Rigidbody rb;

    private Vector3 verticalThrowForce;
    private bool onBeat;
    private bool thrown = false;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        
        verticalThrowForce = new Vector3(0, itemData.verticalThrowForce, 0);
        rb.mass = itemData.mass;
    }

    public void Interact()
    {
        if (GameManager.Instance.IsOffHandFree())
        {
            rb.useGravity = false;
            audioSource.PlayOneShot(visualData.pickupSound);
            GameManager.Instance.SetOffHandItem(this);
        }
    }

    public void Throw(Transform camera)
    {
        onBeat = BeatManager.Instance.CheckOnBeat();
        
        audioSource.PlayOneShot(visualData.throwSound);
        transform.SetParent(null);
        gameObject.layer = LayerMask.NameToLayer("Ground");

        Vector3 throwDirection = CalculateDirection(camera);
        
        Vector3 throwForce = throwDirection * itemData.velocity + verticalThrowForce;
        
        rb.useGravity = itemData.gravity;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        rb.AddForce(throwForce, ForceMode.Impulse);
        thrown = true;
    }

    private Vector3 CalculateDirection(Transform camera)
    {
        Vector3 direction = camera.forward;

        if (Physics.Raycast(camera.position, direction, out RaycastHit hit, 500f))
        {
            direction = (hit.point - transform.position).normalized;
        }
        return direction;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!thrown)
            return;
        
        //TODO: sistema de dano
        switch (itemData.attackType)
        {
            case OffhandAttackType.DirectDamage:
                AudioSource.PlayClipAtPoint(visualData.onHitSound, transform.position);
                DestroyProjectile();
                break;
            case OffhandAttackType.DOTEffect:
                rb.isKinematic = true;
                transform.SetParent(other.transform);
                AudioSource.PlayClipAtPoint(visualData.onHitSound, transform.position);
                Invoke(nameof(DestroyProjectile), 3f);
                break;
            case OffhandAttackType.Explosion:
                Instantiate(explodeEffect, transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(visualData.onHitSound, transform.position);
                DestroyProjectile();
                break;
            case OffhandAttackType.Cloud:
                Instantiate(cloudEffect, transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(visualData.onHitSound, transform.position);
                DestroyProjectile();
                break;
            default:
                Debug.LogError("Missing: AttackType");
                break;
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
