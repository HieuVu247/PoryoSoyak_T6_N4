using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;    

[RequireComponent(typeof(Button))]
public class LevelButtonHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [Tooltip("Tỉ lệ scale khi hover")]
    public float hoverScale = 1.1f;
    [Tooltip("Tỉ lệ scale khi nhấn")]
    public float pressScale = 0.9f;
    [Tooltip("Thời gian tween (s)")]
    public float tweenDuration = 0.2f;
    [Tooltip("Thời gian tween cho press (s)")]
    public float pressDuration = 0.1f;

    private Button _button;
    private RectTransform _rect;
    private Vector3 _originalScale;

    void Awake()
    {
        _button = GetComponent<Button>();
        _rect = GetComponent<RectTransform>();
        _originalScale = _rect.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _rect.DOKill();
        _rect.DOScale(_originalScale * hoverScale, tweenDuration)
             .SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _rect.DOKill();
        _rect.DOScale(_originalScale, tweenDuration)
             .SetEase(Ease.OutBack);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _rect.DOKill();
        _rect.DOScale(_originalScale * pressScale, pressDuration)
             .SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _rect.DOKill();
        bool isHover = RectTransformUtility.RectangleContainsScreenPoint(
            _rect, eventData.position, eventData.pressEventCamera);
        Vector3 targetScale = isHover
            ? _originalScale * hoverScale   
            : _originalScale;               

        _rect.DOScale(targetScale, tweenDuration)
             .SetEase(Ease.OutBack);
    }
}
