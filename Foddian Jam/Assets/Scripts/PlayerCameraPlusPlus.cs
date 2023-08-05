using System;
using Cinemachine;
using UnityEngine;

public class PlayerCameraPlusPlus : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float zoomOutSize = 10;
    [SerializeField] private float zoomInSize = 5f;
    [SerializeField] private float speedThreshold = 5f;

    private Rigidbody2D _playerRb;
    private float _timer;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _playerRb = virtualCamera.Follow.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_playerRb.velocity.magnitude <= speedThreshold)
        {
            _timer += Time.deltaTime;
            virtualCamera.m_Lens.OrthographicSize =
                Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, zoomInSize, _timer);
        }
        else
        {
            _timer += Time.deltaTime;
            virtualCamera.m_Lens.OrthographicSize =
                Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, zoomOutSize, _timer);
        }

        if (Math.Abs(virtualCamera.m_Lens.OrthographicSize - zoomInSize) < 0.1f ||
            Math.Abs(virtualCamera.m_Lens.OrthographicSize - zoomOutSize) < 0.1f)
            _timer = 0f;
    }
}
