using System;
using Offhand.InteractableObjects.ItemData;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private OffhandItemDataSO itemData;
    [SerializeField] private OffhandVisualDataSO visualData;
    
    private AudioSource audioSource;
    private Rigidbody rb;
    private Transform parent;

    private Vector3 verticalThrowForce;
    private bool thrown = false;
    
    private void Start()
    {
        parent = transform.parent;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        
        verticalThrowForce = new Vector3(0, itemData.verticalThrowForce, 0);
        rb.mass = itemData.mass;
    }

    public void Pickup()
    {
        rb.useGravity = false;
        audioSource.PlayOneShot(visualData.pickupSound);
        
    }

    public void Throw(Transform camera)
    {
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
        
        rb.isKinematic = true;
        
        transform.SetParent(other.transform);
        
        audioSource.PlayOneShot(visualData.onHitSound);

        Invoke(nameof(DestroyGameObject), 3f);
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
