using UnityEngine;
using DG.Tweening;

public class BaseMenuActions : MonoBehaviour
{
    public Rigidbody2D car;

    public RectTransform commonPanel;
    public RectTransform pausePanel;
    public RectTransform infoPanel;
    public RectTransform slidePanel;

    private Vector2 _startPausePosition, _startPanelPosition, _startSlidePosition, _startInfoPosition;

    private float _duration;

    private void Start()
    {
        _startPausePosition = pausePanel.anchoredPosition;
        _startPanelPosition = commonPanel.anchoredPosition;
        _startSlidePosition = slidePanel.anchoredPosition;
        _startInfoPosition = infoPanel.anchoredPosition;

        _duration = 1f;
    }

    public void SwitchPanel(RectTransform panelFrom, RectTransform panelTo, Vector2 initPosition, Vector2? targetPosition)
    {
        panelFrom.DOAnchorPos(initPosition, _duration);
        panelTo.DOAnchorPos(targetPosition ?? Vector2.zero, _duration);
    }

    public void SetResume()
    {
        SwitchPanel(pausePanel, commonPanel, _startPausePosition, _startPanelPosition);

        //UnFreeze all positions and rotations
        car.constraints = RigidbodyConstraints2D.None;
    }

    public void SetPause()
    {       
        SwitchPanel(pausePanel, commonPanel, Vector2.zero, Vector2.zero);

        //Freeze all positions and rotations
        car.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void SetSlideMenu() => SwitchPanel(pausePanel,
                                              slidePanel,
                                              _startPausePosition,
                                              new Vector2(-800f, 50f));

    public void SetInfoMenu() => SwitchPanel(slidePanel,
                                             infoPanel,
                                             _startSlidePosition,
                                             null);

    public void BackToPauseMenuFromSlide() => SwitchPanel(slidePanel,
                                                          pausePanel,
                                                          _startSlidePosition,
                                                          null);

    public void BackToPauseMenuFromInfo() => SwitchPanel(infoPanel,
                                                         pausePanel,
                                                         _startInfoPosition,
                                                         null);

    public void BackToSlideMenu() => SwitchPanel(infoPanel,
                                                 slidePanel,
                                                 _startSlidePosition,
                                                 null);

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
