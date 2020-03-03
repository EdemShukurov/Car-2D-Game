using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(WheelJoint2D))]
public class CarBaseMovement : MonoBehaviour, IVehicleMovable
{
    public const float GRAVITY = 9.81f;  

    [Header("Movement properies")]
    [SerializeField] private float brakeForce = 600f;
    [SerializeField] private float maxSpeed = -1500f;
    [SerializeField] private float jumpForce = 9f;
    [SerializeField] private float maxBackSpeed = 600f;
    [SerializeField] private float acceleration = 300f;
    [SerializeField] private float deacceleration = -100f;

    [Header("Limit angle")]
    [SerializeField] private float _minimumZ = -20F;
    [SerializeField] private float _maximumZ = 20F;

    private int _screenWidth;

    protected Rigidbody2D _rigidBody2D;

    private float _movementInput;
    private float _deltaMovement;

    private float _physicValue;

    private float _angleCar;

    private JointMotor2D  _wheelMotor;
    private WheelJoint2D[] wheelJoints;

    private void Start()
    {
        // set screen width
        _screenWidth = Screen.width;

        wheelJoints = (WheelJoint2D[])_rigidBody2D.GetComponents(typeof(WheelJoint2D));

        if (_rigidBody2D == null || wheelJoints == null)
        {
            throw new ArgumentNullException();
        }

        _wheelMotor = wheelJoints[0].motor;
        
    }

    private void Update()
    {
        LimitAngleCar();

        _movementInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetMouseButton(0))
        {
            _deltaMovement = Input.mousePosition.x;
            GetTouch(_deltaMovement);
        }

        UpdateVelocity();
    }

    /// <summary>
    /// Limit car angle in order to avoid car flip
    /// </summary>
    private void LimitAngleCar()
    {
        _angleCar = ClampAngle(transform.eulerAngles.z, _minimumZ, _maximumZ);
        transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, _angleCar);
    }

    /// <summary>
    /// Clamp car angle
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns>permissible car angle</returns>
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;

        if (angle > 360F)
            angle -= 360F;

        if (angle >= 180f)
            angle -= 360f;

        return Mathf.Clamp(angle, min, max);
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
        foreach (var item in wheelJoints)
            item.motor = _wheelMotor;
    }

    public void Brake()
    {
        throw new System.NotImplementedException();
    }

    public void DisableMovement()
    {

    }

    public void Gas()
    {
        throw new System.NotImplementedException();
    }

    public void ReverseGear()
    {
        throw new System.NotImplementedException();
    }
}
