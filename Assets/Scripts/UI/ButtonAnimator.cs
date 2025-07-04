using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class ButtonAnimator : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float pulseScale = 1.1f;
    public float pulseDuration = 2f;

    [Header("Shake Settings")]
    public float shakeDuration = 0.2f;
    public float shakeStrength = 5; 

    private Vector3 originalScale;
    private RectTransform rectTransform;
    private Sequence pulseSequence;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    private void OnEnable()
    {
        StartPulseAndShake();
    }

    private void OnDisable()
    {
        if (pulseSequence != null && pulseSequence.IsActive())
            pulseSequence.Kill();
    }

    private void StartPulseAndShake()
    {
        rectTransform.localScale = originalScale;
        rectTransform.localRotation = Quaternion.identity;

        pulseSequence = DOTween.Sequence();
        pulseSequence.Append(rectTransform.DOScale(originalScale * pulseScale, pulseDuration / 2).SetEase(Ease.OutSine))
                     .Append(rectTransform.DOScale(originalScale, pulseDuration / 2).SetEase(Ease.InSine))
                     .Append(rectTransform.DOShakeRotation(shakeDuration, new Vector3(0, 0, shakeStrength), vibrato: 10, randomness: 90f, fadeOut: true))
                     .SetLoops(-1);
    }
}
