using System;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    public void SetCameraFOV(float fov, float duration)
    {
        _camera.DOFieldOfView(fov, duration);
    }
}
