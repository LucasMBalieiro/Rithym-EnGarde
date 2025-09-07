using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TestBeat : MonoBehaviour
{
    [SerializeField] private bool testStart;
    [SerializeField] private float beatSize;
    [SerializeField] private float returnSpeed;

    private Vector3 _localScale;
    private Tweener _scaleTweener;


    private void Start()
    {
        _localScale = transform.localScale;

        if (testStart)
        {
            StartCoroutine(DebugBeat());
        }
        
    }

    private void Update()
    {
        //transform.localScale = Vector3.Lerp(transform.localScale, _localScale, returnSpeed * Time.deltaTime);
        ReturnOriginalScale();
    }

    private void ReturnOriginalScale()
    {
        if (transform.localScale != _localScale)
        {
            _scaleTweener = transform.DOScale(_localScale, returnSpeed);
        }
    }

    public void Beat()
    {
        transform.localScale = _localScale * beatSize;
    }

    private IEnumerator DebugBeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            
            Beat();
        }
    }
}
