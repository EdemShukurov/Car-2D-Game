using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(WheelJoint2D))]
public class CarBaseMovement : MonoBehaviour
{
    public const float GRAVITY = 9.81f;

    protected Rigidbody2D _rigidBody2D;
    protected float _movementInput;

    private readonly float _maxSpeed = 50f;
    private readonly float _brakeTime = 0.75f;
    private readonly float _brakeDrag = 3f;

    private static IAngleVehicleRotation _carRotation;
    private Speed _speed;
    
    private int _centerScreenX;

    private enum Speed
    {
        Increase,
        Decrease,
        Brake,
        NotAction
    }

    private void Start()
    {
        // set center screen width
        _centerScreenX = Screen.width / 2;

        _rigidBody2D = GetComponent<Rigidbody2D>();
        _carRotation = GetComponent<CarRotation>();

        _speed = Speed.NotAction;
    }

    protected virtual void Update()
    {
        Debug.Log("virt");
        _carRotation.LimitAngleCar();

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            _movementInput = GetTouch(Input.mousePosition.x);
        }
        else
        {
            _movementInput = 0f;
        }

        DetermineAction();
    }

    private void DetermineAction()
    {
        if (UseBrake())
        {
            Brake();
            _speed = Speed.Brake;
           
            return;
        }
        else if (_movementInput == 0f)
        {
            _speed = Speed.NotAction;
        }
        else if (_movementInput < 0f)
        {
            _speed = Speed.Decrease;
        }
        else
        {
            _speed = Speed.Increase;
        }
        
        UpdateVelocity();
    }

    /// <summary>
    /// if we suddenly changed direction, we should break
    /// </summary>
    /// <returns></returns>
    private bool UseBrake() => 
        (_movementInput > 0f && _speed == Speed.Decrease) || 
        (_movementInput < 0f && _speed == Speed.Increase) || 
        Input.GetKey(KeyCode.Space);


    /// <summary>
    /// Get touch/mouseclick position to determine speed
    /// /// </summary>
    /// <param name="touchPosition">touch/mouseclick position</param>
    protected float GetTouch(float touchPosition) => _movementInput = (touchPosition - _centerScreenX) / _centerScreenX;

    /// <summary>
    /// Update car velocity
    /// </summary>
    private void UpdateVelocity()
    {
        if (_speed == Speed.Decrease || _speed == Speed.Increase)
            _rigidBody2D.drag = 0f;

        // speed is always in this limits, Clamp is needed to avoid NaN excepion while forcing, 
        // as _movementInput may be (in unityedit mode) more 1 or less -1
        float speed = Mathf.Clamp(_maxSpeed * _movementInput, -_maxSpeed, _maxSpeed);
        
        _rigidBody2D.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
    }

    public void Brake()
    {
        StartCoroutine(SmoothBrake(_brakeTime));
    }

    /// <summary>
    /// Use Coroutine in order to rotate pivot smoothly
    /// </summary>
    /// <param name="targetAngle"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator SmoothBrake(float time)
    {
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            _rigidBody2D.drag = _brakeDrag;

            yield return null;
        }
    }

    private float GetPhysicInfluenceValue() => GRAVITY * Mathf.Sin((transform.eulerAngles.z * Mathf.PI) / 180f) * 80f;
}