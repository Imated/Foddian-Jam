using DG.Tweening;
using UnityEngine;

public class UIMoveY : MonoBehaviour
{
    [SerializeField] private float newY;
    [SerializeField] private float scaleDuration = 0.15f;
    [SerializeField] private Ease moveEase;

    private float _defaultY;
    private bool _isUp;

    private void Awake()
    {
        _defaultY = transform.GetComponent<RectTransform>().anchoredPosition.y;
    }

    public void Tween()
    {
        if (!_isUp)
        {
            transform.GetComponent<RectTransform>().DOAnchorPosY(newY, scaleDuration).SetEase(moveEase);
            _isUp = true;
        }
        else
        {
            transform.GetComponent<RectTransform>().DOAnchorPosY(_defaultY, scaleDuration).SetEase(moveEase);
            _isUp = false;
        }
    }
}
