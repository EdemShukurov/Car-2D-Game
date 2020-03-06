using UnityEngine;
using DG.Tweening;

public class BaseMenuActions : MonoBehaviour
{
    public RectTransform commonCanvas;
    public RectTransform pausePanel;
    public RectTransform infoPanel;
    public RectTransform slidePanel;

    private Vector2 _startPosition, _targetPosition;

    private RectTransform _currentPanel;

    private float _duration, _timeScale;

    private void Start()
    {
        _startPosition = new Vector2(0f, 500f);
        _targetPosition = Vector2.zero;

        _duration = _timeScale = 1f;

        //pausePanel.DOKill();
        //commonCanvas.DOKill();
        //slidePanel.DOKill();
        //infoPanel.DOKill();

    }

    public void SetResume()
    {
        Tween tween1 = pausePanel.DOAnchorPos(_startPosition, _duration);
        tween1?.Kill();
        Tween tween2 = commonCanvas.DOAnchorPos(_startPosition, _duration);
        tween2?.Kill();

        _currentPanel = null;

        Invoke("RefreshTime", _duration);
    }

    public void SetPause()
    {
        Tween tween1 = pausePanel.DOAnchorPos(_targetPosition, _duration);
        tween1?.Kill();
        Tween tween2 = commonCanvas.DOAnchorPos(_targetPosition, _duration);
        tween2?.Kill();

        _currentPanel = pausePanel;

        Invoke("BrakeTime", _duration);
    }

    private void BrakeTime()
    {
        Time.timeScale = 0f;
    }

    private void RefreshTime()
    {
        Time.timeScale = 1F;
    }

    public void OpenWindow(RectTransform windowToClose)
    {
        Tween tween2 = windowToClose.DOAnchorPos(Vector2.zero, _duration);
        tween2?.Kill();

        CloseWindow(_currentPanel);
    }

    public void CloseWindow(RectTransform windowToOpen)
    {
        Tween tween2 = windowToOpen.DOAnchorPos(_startPosition, _duration);
        tween2?.Kill();
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
