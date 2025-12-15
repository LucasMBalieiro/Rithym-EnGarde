using Audio;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] SoundData hoverButtonSound;
    [SerializeField] SoundData pressedButtonSound;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.CreateSound().Play(hoverButtonSound);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.CreateSound().Play(pressedButtonSound);
    }
}
