using DG.Tweening;
using Rhythm._Referee;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Rhythm.View
{
    public class RhythmView : MonoBehaviour
    { 
        [SerializeField] private Image aimReticle;

        [Header("Feedback Colors")] 
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color successColor;
        [SerializeField] private Color errorColor;
        
        private void Start()
        {
            ResetReticleColor();
            
            BeatManager.BeatEnter += ExpandReticle;
            BeatManager.BeatExit += RetractReticle;
            BeatManager.AttackOnBeat += FeedbackInput;
        }

        private void ExpandReticle()
        {
            aimReticle.transform.DOScale(1.4f, 0.0001f);
        }

        private void RetractReticle()
        {
            aimReticle.transform.DOScale(1f, 0.0001f);
        }

        private void FeedbackInput(bool beat)
        {
            aimReticle.color = (beat ? successColor : errorColor);
            Invoke(nameof(ResetReticleColor), .1f);
        }

        public void ResetReticleColor()
        {
            aimReticle.color = defaultColor;
        }
    }
}
