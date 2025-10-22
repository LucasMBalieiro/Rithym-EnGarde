using System;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    private Tween tiltTween;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    public void DoFov(float fov, float duration)
    {
        _camera.DOFieldOfView(fov, duration);
    }
    
    public void DoTilt(float zTilt, float duration)
    {
        tiltTween?.Kill();
        tiltTween = transform.DOLocalRotate(new Vector3(0f, 0f, zTilt), duration);
    }
}
