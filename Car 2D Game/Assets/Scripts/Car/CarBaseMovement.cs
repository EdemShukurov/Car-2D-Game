using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(WheelJoint2D))]
public class CarBaseMovement : MonoBehaviour
{
    public const float GRAVITY = 9.81f;

    protected Rigidbody2D _rigidBody2D;

    [Header("Wheels Joint")]
    [SerializeField] private WheelJoint2D _frontWheelJoint;
    [SerializeField] private WheelJoint2D _backWheelJoint;


    private static IAngleVehicleRotation _carRotation;

    #region Movement Propeties
    private readonly float _brakeForce = 1000f;
    private readonly float _maxSpeed = 200f;
    private readonly float _maxBackSpeed = 1000f;
    #endregion

    private int _centerScreenX;
    private float _movementInput;
    private float _physicValue;

    private JointMotor2D _wheelMotor;

    private void Start()
    {
        // set center screen width
        _centerScreenX = Screen.width / 2;

        _rigidBody2D = GetComponent<Rigidbody2D>();

        if (_rigidBody2D == null || _frontWheelJoint == null || _backWheelJoint == null)
        {
            throw new ArgumentNullException();
        }

        _wheelMotor = _backWheelJoint.motor;
        _carRotation = GetComponent<CarRotation>();
    }

    protected virtual void Update()
    {
        _carRotation.LimitAngleCar();

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            GetTouch(Input.mousePosition.x);
            SetUseMotor(false);
        }
        else
        {
            _movementInput = 0f;
        }

        UpdateVelocity();
    }

    /// <summary>
    /// Get touch/mouseclick position to determine speed via _deltaMovement variable
    /// </summary>
    /// <param name="touchPosition">touch/mouseclick position</param>
    protected void GetTouch(float touchPosition)
    {
        if (touchPosition != _centerScreenX)
        {
            _movementInput = (touchPosition - _centerScreenX) / _centerScreenX;
        }
        else
        {
            _movementInput = .0f;
        }
    }

    /// <summary>
    /// Update car velocity
    /// </summary>
    private void UpdateVelocity()
    {
        _rigidBody2D.AddForce(-Vector2.left * _maxSpeed * _movementInput, ForceMode2D.Impulse);

        if (Input.GetKey(KeyCode.Space))
            Brake();
    }

    /// <summary>
    /// Set wheels motor speed
    /// </summary>
    private void SetWheelsMotorSpeed()
    {
        SetUseMotor(true);
        _frontWheelJoint.motor = _backWheelJoint.motor = _wheelMotor;
    }

    /// <summary>
    /// Set UseMotor for every WheelJoint2D
    /// </summary>
    /// <param name="useMotor"></param>
    private void SetUseMotor(bool useMotor)
    {
        _frontWheelJoint.useMotor = _backWheelJoint.useMotor = useMotor;
    }

    public void Brake()
    {
        //_movementInput = 0f;
        Debug.LogWarning("Brake");
        if (_wheelMotor.motorSpeed > 0f)
        {
            _wheelMotor.motorSpeed = Mathf.Clamp(
                _wheelMotor.motorSpeed - _brakeForce * Time.deltaTime,
                0f,
                _maxBackSpeed);
        }

        if (_wheelMotor.motorSpeed < 0f)
        {
            _wheelMotor.motorSpeed = Mathf.Clamp(
                _wheelMotor.motorSpeed + _brakeForce * Time.deltaTime,
                _maxSpeed,
                0f);
        }

        SetWheelsMotorSpeed();
    }

    private float GetPhysicInfluenceValue()
    {
        return GRAVITY * Mathf.Sin((transform.eulerAngles.z * Mathf.PI) / 180f) * 80f;
    }
}