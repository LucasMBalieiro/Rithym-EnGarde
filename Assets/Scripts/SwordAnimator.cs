using System;
using UnityEngine;
using DG.Tweening;

public class SwordAnimator : MonoBehaviour
{
    [SerializeField] private Transform sword, swordTip;       // Objeto da espada
    [SerializeField] private Transform idlePosition; // Posição inicial (pai vazio que guarda a pose de idle)
    [SerializeField] private Transform target;       // Transform do alvo
    [SerializeField] private float stabDuration = 0.2f;
    [SerializeField] private float returnDuration = 0.25f;
    [SerializeField] private float overshoot = 0.1f; // Quanto passar do alvo (opcional, para dar impacto)

    private Tween currentTween;

    private void Awake()
    {
        currentTween = null;
        swordTip.localPosition = idlePosition.localPosition;
        swordTip.rotation = idlePosition.rotation;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DoStab()
    {
        if (currentTween != null && currentTween.IsActive())
            return;
        
        Vector3 direction = (target.position - swordTip.localPosition).normalized;
        Quaternion lookRot = Quaternion.LookRotation(direction);

        currentTween = DOTween.Sequence()
            .Append(swordTip.DORotateQuaternion(lookRot, stabDuration).SetEase(Ease.OutQuad))
            .Append(swordTip.DOMove(target.position - direction * 0.2f, stabDuration));
        currentTween.SetLoops(2, LoopType.Yoyo);
    }
}