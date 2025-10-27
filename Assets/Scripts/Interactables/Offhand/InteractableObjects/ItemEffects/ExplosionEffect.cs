using DG.Tweening;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private Sequence explodeSequence;
    private MeshRenderer meshRenderer;
    
    private Vector3 startScale;
    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        startScale = transform.localScale;
        
        explodeSequence = DOTween.Sequence();
        explodeSequence.Append(transform.DOScale(startScale * 1.5f, 0.5f));
        explodeSequence.Join(meshRenderer.material.DOFade(0, 0.5f));
        explodeSequence.OnComplete((() => { Destroy(gameObject); }));
    }
}
