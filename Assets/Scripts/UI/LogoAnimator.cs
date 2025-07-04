using UnityEngine;
using DG.Tweening;

public class LogoAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    public float scaleDuration = 1f;
    public float punchScale = 0.2f;
    public float loopInterval = 2f;

    private Vector3 targetScale;

    private void Awake()
    {
        targetScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        transform.DOScale(targetScale, scaleDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(StartLooping);
    }

    private void StartLooping()
    {
        transform.DOPunchScale(Vector3.one * punchScale, scaleDuration, 1, 0.5f)
            .SetDelay(loopInterval)
            .OnComplete(StartLooping);
    }
}
