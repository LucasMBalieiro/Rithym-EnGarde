using System;
using Combat.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Combo
{
    public class ComboView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup frame;
        [SerializeField] private TextMeshProUGUI comboIndicator;
        [SerializeField] private TextMeshProUGUI comboText;
        [SerializeField] private Image comboImage;

        private Sequence _animatedUpdate;
        
        public void Start()
        {
            ComboManager.OnComboView += UpdateComboView;
            HideUI();
        }

        public void OnDisable()
        {
            ComboManager.OnComboView -= UpdateComboView;
        }

        private void UpdateComboView()
        {
            var comboValue = CombatDataStorage.Combo;
            if (comboValue == 0)
            {
                HideUI();
                return;
            }
            
            if (!frame.interactable)
            {
                ShowUI();
            }
            
            var comboViewData = CombatDataStorage.CurrentComboState;
            var newComboState = comboText.text != comboViewData.stateDescription;
            
            var animationRoot = comboIndicator.transform;
            if (newComboState)
            {
                animationRoot = comboImage.transform;
            }
            
            _animatedUpdate = DOTween.Sequence();
            _animatedUpdate.Append(animationRoot.DOScale(1.2f, 0.2f));
            _animatedUpdate.AppendCallback(() => SetView(comboValue, comboViewData, newComboState));
            _animatedUpdate.Append(animationRoot.DOScale(1f, 0.2f));
        }

        private void SetView(int comboValue, ComboState comboViewData, bool newComboState)
        {
            comboIndicator.text = $"{comboValue: 000}";
            comboIndicator.color = comboViewData.thresholdColor;

            if (newComboState)
            {
                comboImage.sprite = comboViewData.frame;
                    
                comboText.text = comboViewData.stateDescription;
                comboText.color = comboViewData.thresholdColor;
            }
        }
        
        private void ShowUI()
        {
            frame.interactable = true;
            frame.blocksRaycasts = true;
            frame.alpha = 1f;
        }
        
        private void HideUI()
        {
            if (_animatedUpdate is not null && _animatedUpdate.IsPlaying())
                _animatedUpdate.Kill(complete: true);
                
            frame.alpha = 0f;
            frame.interactable = false;
            frame.blocksRaycasts = false;
        }
    }
}
