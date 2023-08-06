using System;
using Cinemachine;
using UnityEngine;

public class PlayerCameraPlusPlus : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float zoomOutSize;
    [SerializeField] private float zoomInSize;
    [SerializeField] private float minSpeedThreshold;
    [SerializeField] private float maxSpeedThreshold;
    [SerializeField] float zoomSpeed;

    [SerializeField] Rigidbody2D _playerRb;
    //private float _timer;

    public bool cameraLock;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _playerRb = virtualCamera.Follow.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Mapping speed value to a target zoom value
        float speedClamp = Mathf.Clamp(_playerRb.velocity.magnitude, minSpeedThreshold, maxSpeedThreshold);
        float proportion = (speedClamp - minSpeedThreshold) / (maxSpeedThreshold - minSpeedThreshold);
        float targetZoom = proportion * (zoomOutSize - zoomInSize) + zoomInSize;

        // Using lerp to smooth out the camera changes
        virtualCamera.m_Lens.OrthographicSize =
            Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

        //if (_playerRb.velocity.magnitude <= speedThreshold)
        //{
        //    _timer += Time.deltaTime;
        //    virtualCamera.m_Lens.OrthographicSize =
        //        Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, zoomInSize, _timer);
        //}
        //else
        //{
        //    _timer += Time.deltaTime;
        //    virtualCamera.m_Lens.OrthographicSize =
        //        Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, zoomOutSize, _timer);
        //}

        //if (Math.Abs(virtualCamera.m_Lens.OrthographicSize - zoomInSize) < 0.1f ||
        //    Math.Abs(virtualCamera.m_Lens.OrthographicSize - zoomOutSize) < 0.1f)
        //    _timer = 0f;
    }
}
