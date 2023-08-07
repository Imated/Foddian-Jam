using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Spedometer : MonoBehaviour
{
    [SerializeField] private Color completeColor = Color.green;
    [SerializeField] private Image fillImage;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float lerpSpeed = 1f;
    [SerializeField] private float max = 100f;
    [SerializeField] private bool overwriteSpeed = true;
    [SerializeField] private float value;
    
    private RectTransform _fillImageRect;
    private float _currentValue;

    private void Awake()
    {
        _fillImageRect = fillImage.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(!overwriteSpeed)
            value = playerRb.velocity.magnitude;
        var normalizedValue = value / max;
        if (Math.Abs(normalizedValue - 1f) < 0.01f)
            fillImage.color = completeColor;
        else
            fillImage.color = Color.white;
        var topValue = Mathf.Lerp(460, 30, normalizedValue);
        _currentValue = Mathf.Lerp(_currentValue, topValue, Time.deltaTime * lerpSpeed);
        _fillImageRect.offsetMax = new Vector2(_fillImageRect.offsetMax.x, -_currentValue);
    }
}
