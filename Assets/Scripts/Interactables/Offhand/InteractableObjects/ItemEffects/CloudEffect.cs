using DG.Tweening;
using UnityEngine;

public class CloudEffect : MonoBehaviour
{
    private Sequence cloudSequence;
    private MeshRenderer meshRenderer;
    
    private Vector3 startScale;
    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        
        startScale = transform.localScale;
        
        cloudSequence = DOTween.Sequence();
        cloudSequence.Append(transform.DOScale(startScale * 1.5f, 1f));
        cloudSequence.Append(transform.DOScale(startScale, 1f));
        cloudSequence.Append(transform.DOScale(startScale * 1.5f, 1f));
        cloudSequence.Append(transform.DOScale(startScale, 1f));
        cloudSequence.Join(meshRenderer.material.DOFade(0, 1f));
        cloudSequence.OnComplete((() => { Destroy(gameObject); }));
    }
}
