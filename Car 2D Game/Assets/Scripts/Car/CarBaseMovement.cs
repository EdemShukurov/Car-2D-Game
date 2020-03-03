using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CarBaseMovement : MonoBehaviour, IVehicleMovable
{
    public Rigidbody2D rigidBody2D;
    
    private int _screenWidth;
    private float _movementInput;
    private float _deltaMovement;

    private JointMotor2D _frontWheelMotor, _backWheelMotor;
    private WheelJoint2D[] wheelJoints;

    private void Start()
    {
        // set screen width
        _screenWidth = Screen.width;

        wheelJoints = (WheelJoint2D[])rigidBody2D.GetComponents(typeof(WheelJoint2D));

        if (rigidBody2D == null || wheelJoints == null)
        {
            throw new ArgumentNullException();
        }

        _frontWheelMotor = wheelJoints[0].motor;
        _backWheelMotor = wheelJoints[1].motor;
    }

    private void Update()
    {
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
                if (_backWheelMotor.motorSpeed >= 0)
                    ReverseGear();
                else
                    Brake();
                break;

            case 0f:
                DisableMovement();
                break;

            case 1f:
                if (_backWheelMotor.motorSpeed <= 0)
                    Gas();
                else
                    Brake();
                break;
        }

        if (Input.GetKey(KeyCode.Space))
            Brake();


        foreach (var item in wheelJoints)
            item.motor = _backWheelMotor;
    }

    public void Brake()
    {
        throw new System.NotImplementedException();
    }

    public void DisableMovement()
    {
        throw new System.NotImplementedException();
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
