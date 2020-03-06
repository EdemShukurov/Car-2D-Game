using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum SlideDirection
{
    Next,
    Previous
}


public class ImageSlider : MonoBehaviour
{
    private float _offsetSlide;
    private RectTransform _rectTransform;

    private void Start()
    {
        _offsetSlide = 34f;

        _rectTransform = GetComponent<RectTransform>();
        var childs = GetComponentsInChildren<HorizontalLayoutGroup>();

    }

    public void SlideForward()
    {
        _rectTransform.DOAnchorPos(new Vector2(gameObject.transform.position.x + _offsetSlide, 0f ) ,1f);
    }

    public void SlideBack()
    {
        _rectTransform.DOAnchorPos(new Vector2(gameObject.transform.position.x - _offsetSlide, 0f), 1f);
    }

}
