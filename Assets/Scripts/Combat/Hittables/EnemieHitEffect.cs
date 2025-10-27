using System;
using DG.Tweening;
using UnityEngine;

namespace Combat.Hittables
{
    public class EnemieHitEffect : MonoBehaviour, IHitEffect
    {
        private Sequence _animation;

        private void Awake()
        {
            _animation = DOTween.Sequence();
            _animation.OnKill(() => transform.localScale = Vector3.one);
        }

        private void OnDisable()
        {
            _animation.Kill(complete: false);
        }

        public void ApplyHitEffect(float hitValue)
        {
            _animation.Kill();
            
            _animation = DOTween.Sequence();
            _animation.Append(transform.DOScale(.75f, .2f));
            _animation.Append(transform.DOScale(1f, .2f));
            
            Debug.Log($"Dealing {hitValue} to {gameObject.name}");
        }

        public void ApplyContactEffect(Vector3 point)
        {
            return;
        }
    }
}
