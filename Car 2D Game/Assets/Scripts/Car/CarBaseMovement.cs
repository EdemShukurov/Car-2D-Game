using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(WheelJoint2D))]
public class CarBaseMovement : MonoBehaviour, IVehicleMovable
{
    public const float GRAVITY = 9.81f;

    [Header("Wheels Joint")]

    [SerializeField] protected WheelJoint2D _frontWheelJoint;
    [SerializeField] protected WheelJoint2D _backWheelJoint;

    [Header("Movement properies")]
 
    [SerializeField] private float brakeForce = 800f;
    [SerializeField] private float maxSpeed = -3500f;
    [SerializeField] private float jumpForce = 9f;
    [SerializeField] private float maxBackSpeed = 1000f;
    [SerializeField] private float acceleration = 500f;
    [SerializeField] private float deacceleration = -300f;

    private int _centerScreenX;

    protected Rigidbody2D _rigidBody2D;

    private float _movementInput;
    private float _deltaMovement;

    private float _physicValue;

    private JointMotor2D _wheelMotor;

    private static IAngleCarRotation _carRotation;

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

       // _movementInput = Input.GetAxis("Horizontal");



        if (Input.GetMouseButton(0))
        {
            _deltaMovement = Input.mousePosition.x;
            GetTouch(_deltaMovement);

            UpdateVelocity();

            SetWheelsMotorSpeed();
        }


    }
 

    /// <summary>
    /// Get touch/mouseclick position to determine speed via _deltaMovement variable
    /// </summary>
    /// <param name="touchPos">touch/mouseclick position</param>
    protected void GetTouch(float touchPos)
    {
        if (touchPos > _centerScreenX)
        {
            _movementInput = touchPos - _centerScreenX;
        }
        if (touchPos < _centerScreenX)
        {
            _movementInput = _centerScreenX - touchPos;
        }

        print("touchPos " + touchPos + "_movementInput   " + _movementInput);
    }

    /// <summary>
    /// Update car velocity
    /// </summary>
    private void UpdateVelocity()
    {
        //determine action
        //switch (_movementInput)
        //{
        //    case -1f:
        //        if (_wheelMotor.motorSpeed >= 0)
        //            ReverseGear();
        //        else
        //            Brake();
        //        break;

        //    case 0f:
        //        DisableMovement();
        //        break;

        //    case 1f:
        //        if (_wheelMotor.motorSpeed <= 0)
        //            Gas();
        //        else
        //            Brake();
        //        break;
        //}
        SetVelocity();

        if (Input.GetKey(KeyCode.Space))
            Brake();
    }

    /// <summary>
    /// Set wheels motor speed
    /// </summary>
    private void SetWheelsMotorSpeed()
    {
        _frontWheelJoint.motor = _backWheelJoint.motor = _wheelMotor;
    }

    /// <summary>
    /// Set velocity 
    /// </summary>
    private void SetVelocity()
    {
        _physicValue = GRAVITY * Mathf.Sin((transform.eulerAngles.z * Mathf.PI) / 180f) * 80f;

        //_wheelMotor.motorSpeed = Mathf.Clamp(
        //    _wheelMotor.motorSpeed - (acceleration * _movementInput * _physicValue) * Time.deltaTime,
        //    maxSpeed,
        //    0);

        _wheelMotor.motorSpeed = Mathf.Clamp(
            _wheelMotor.motorSpeed - ( _movementInput - _physicValue) * Time.deltaTime,
            -7000f,
            7000f);
    }

    public void Brake()
    {
        if (_wheelMotor.motorSpeed > 0f)
        {
            _wheelMotor.motorSpeed = Mathf.Clamp(
                _wheelMotor.motorSpeed - brakeForce * Time.deltaTime,
                0f,
                maxBackSpeed);
        }

        if (_wheelMotor.motorSpeed < 0f)
        {
            _wheelMotor.motorSpeed = Mathf.Clamp(
                _wheelMotor.motorSpeed + brakeForce * Time.deltaTime,
                maxSpeed,
                0f);
        }
    }

    public void DisableMovement()
    {
        _physicValue = GRAVITY * Mathf.Sin((transform.eulerAngles.z * Mathf.PI) / 180f) * 80f;

        if (_wheelMotor.motorSpeed < 0f || (_wheelMotor.motorSpeed == 0f && transform.eulerAngles.z < 0f))
        {
            _wheelMotor.motorSpeed = Mathf.Clamp(
                _wheelMotor.motorSpeed - (deacceleration - _physicValue) * Time.deltaTime,
                maxSpeed,
                0f);
        }

        if (_wheelMotor.motorSpeed > 0f || (_wheelMotor.motorSpeed == 0f && transform.eulerAngles.z > 0f))
        {
            _wheelMotor.motorSpeed = Mathf.Clamp(
                _wheelMotor.motorSpeed - (-deacceleration - _physicValue) * Time.deltaTime,
                0f,
                maxBackSpeed);
        }
    }

    public void Gas()
    {
        _physicValue = GRAVITY * Mathf.Sin((transform.eulerAngles.z * Mathf.PI) / 180f) * 80f;

        _wheelMotor.motorSpeed = Mathf.Clamp(
            _wheelMotor.motorSpeed - (acceleration + _deltaMovement - _physicValue) * Time.deltaTime,
            maxSpeed,
            0);
    }

    public void ReverseGear()
    {
        if (_wheelMotor.motorSpeed > 0f)
        {
            _wheelMotor.motorSpeed = Mathf.Clamp(
                _wheelMotor.motorSpeed + brakeForce * Time.deltaTime, //- _deltaMovement,
                0f,
                maxBackSpeed);
        }

        if (_wheelMotor.motorSpeed == 0f)
        {
            _wheelMotor.motorSpeed = _wheelMotor.motorSpeed + brakeForce / 10f; //- _deltaMovement;
        }
    }
}