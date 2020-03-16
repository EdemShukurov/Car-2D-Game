using UnityEngine;

public class TouchManager : MonoBehaviour
{
    #region singletonStuff

    private static TouchManager _instance;

    public static TouchManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<TouchManager>();
            return _instance;
        }
    }

    #endregion singletonStuff

    public delegate void TouchDelegate(Touch eventData);

    public static event TouchDelegate OnTouchDown;

    public static event TouchDelegate OnTouchUp;

    //public static event TouchDelegate OnTouchDrag;
    public static event TouchDelegate OnTouchStationaryOrMoved;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchDown?.Invoke(touch);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    OnTouchUp?.Invoke(touch);
                    break;

                default:
                    OnTouchStationaryOrMoved?.Invoke(touch);
                    break;
            }
        }
    }
}