﻿using UnityEngine;
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
        _offsetSlide = 275f;

        _rectTransform = GetComponent<RectTransform>();
    }

    public void SlideForward()
    {
        _rectTransform.DOAnchorPosX(_rectTransform.anchoredPosition.x + _offsetSlide, 1f);
    }

    public void SlideBack()
    {
        _rectTransform.DOAnchorPosX(_rectTransform.anchoredPosition.x - _offsetSlide, 1f);
    }

}
