using DG.Tweening;
using UnityEngine;

public class Note : MonoBehaviour
{
    private Sequence moveTween;
    private float travelTime;
    private bool wasHit = false;
    
    public static Note HittableNote { get; private set; }

    private void OnEnable()
    {
        wasHit = false;
    }
    
    private void OnDisable()
    {
        moveTween?.Kill(); 
        
        if (HittableNote == this)
        {
            HittableNote = null;
        }
    }
    
    public void Initialize(Vector3 startPos, Vector3 targetPos, float timeToTravel)
    {
        transform.position = startPos;
        this.travelTime = timeToTravel;


        Vector3 velocity = (targetPos - startPos) / timeToTravel;

        float totalDuration = timeToTravel * 1.5f;

        Vector3 finalEndPos = startPos + (velocity * totalDuration);

        moveTween = DOTween.Sequence();
        
        moveTween.Append(transform.DOMove(finalEndPos, totalDuration)
            .SetEase(Ease.Linear));

        moveTween.InsertCallback(this.travelTime * 0.6f, OnEnterHitWindow);
        moveTween.InsertCallback(this.travelTime * 1.4f, OnExitHitWindow);
        
        moveTween.OnComplete(() => GameManager.Instance.ReturnNote(gameObject));
    }

    private void OnEnterHitWindow()
    {
        if(!wasHit) HittableNote = this;
    }

    private void OnExitHitWindow()
    {
        if (HittableNote == this)
        {
            HittableNote = null;
        }
    }
    
    public void RegisterHit()
    {
        if (wasHit) return;

        wasHit = true;
        HittableNote = null; 
    }
    
    public float GetHitAccuracy()
    {

        float elapsedTime = moveTween.Elapsed();
    
        float error = Mathf.Abs(1.0f - (elapsedTime / this.travelTime));

        return Mathf.Max(0f, 1f - error);
    }
    
}
