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
 
    [SerializeField] private float brakeForce = 600f;
    [SerializeField] private float maxSpeed = -1500f;
    [SerializeField] private float jumpForce = 9f;
    [SerializeField] private float maxBackSpeed = 600f;
    [SerializeField] private float acceleration = 300f;
    [SerializeField] private float deacceleration = -100f;


    private int _screenWidth;

    protected Rigidbody2D _rigidBody2D;

    private float _movementInput;
    private float _deltaMovement;

    private float _physicValue;

    private JointMotor2D _wheelMotor;

    private static IAngleCarRotation _carRotation;

    private void Start()
    {
        // set screen width
        _screenWidth = Screen.width;
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

        _movementInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetMouseButton(0))
        {
            _deltaMovement = Input.mousePosition.x;
            GetTouch(_deltaMovement);
        }

        UpdateVelocity();
    }
 

    /// <summary>
    /// Get touch/mouseclick position to determine speed via _deltaMovement variable
    /// </summary>
    /// <param name="touchPos">touch/mouseclick position</param>
    protected void GetTouch(float touchPos)
    {
        if (touchPos > _screenWidth / 2f)
        {
            _movementInput = 1.0f;
        }
        if (touchPos < _screenWidth / 2f)
        {
            _movementInput = -1.0f;
        }
    }

    /// <summary>
    /// Update car velocity
    /// </summary>
    private void UpdateVelocity()
    {
        //determine action
        switch (_movementInput)
        {
            case -1f:
                if (_wheelMotor.motorSpeed >= 0)
                    ReverseGear();
                else
                    Brake();
                break;

            case 0f:
                DisableMovement();
                break;

            case 1f:
                if (_wheelMotor.motorSpeed <= 0)
                    Gas();
                else
                    Brake();
                break;
        }

        if (Input.GetKey(KeyCode.Space))
            Brake();

        //set wheels motor speed
        _frontWheelJoint.motor = _backWheelJoint.motor = _wheelMotor;
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