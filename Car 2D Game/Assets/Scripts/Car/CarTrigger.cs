using UnityEngine;
using System;
using TMPro;
using System.Collections;

public class CarTrigger : MonoBehaviour
{
    public event Action OnCoinCounterChanged;
    public event Action OnFinished;

    public TextMeshProUGUI text;
    public Rigidbody2D carRigidBody;

    private int _amount;


    private Capture _capture;

    private void Start()
    {
        _capture = Capture.Instance;
    }

    public void AddCoin()
    {
        _amount++;
        text.text = string.Format("X {0}", _amount);
        OnCoinCounterChanged?.Invoke();
    }

    public void SmoothBrakeCar()
    {
        StartCoroutine(SmoothBrake(3.5f));

        Invoke("MakeScreenShot", 2f);
        OnFinished?.Invoke();
    }

    private void MakeScreenShot()
    {
        _capture.MakeScreenShot();

    }

    /// <summary>
    /// Use Coroutine in order to brake smoothly
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator SmoothBrake(float time)
    {
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            carRigidBody.drag = 2f;

            yield return null;
        }
    }
}


