using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class DoTweenRectTransformMove : MonoBehaviour
{
    [SerializeField] Vector2 from;
    [SerializeField] Vector2 to;
    [SerializeField] float Time;
    [SerializeField] float DelayTime;
    [SerializeField] Ease easeType;

    public UnityAction endCallback;

    void OnEnable()
    {
        RectTransform rt = transform as RectTransform;
        rt.anchoredPosition = from;
        rt.DOAnchorPos(to, Time).SetDelay(DelayTime).SetEase(easeType).OnComplete(OnComplete);
    }

    public void RegisterEndCallback(UnityAction ua)
    {
        endCallback = ua;
    }

    void OnComplete()
    {
        if (endCallback != null)
            endCallback.Invoke();
    }
}
