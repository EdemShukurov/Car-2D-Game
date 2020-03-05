using System;
using UnityEngine;

public class CarJumping : CarBaseMovement
{
    [Header("Wheels")]
    public WheelCollision _frontWheelCollision;
    public WheelCollision _backWheelCollision;

    [SerializeField] private float _jumpForce = 1500f;
    private bool _isJumping;
    private bool _isFalling;

    protected override void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Jump();
        }
        else
        {
            base.Update();
        }
    }

    private void Jump()
    {
        // Set falling flag
        if (_isJumping && _rigidBody2D.velocity.y < 0)
        {
            _isFalling = true;
        }

        // Jump
        if (_frontWheelCollision.isGrounded && _backWheelCollision.isGrounded)
        {
            // Jump using impulse force
            _rigidBody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

            // Set jumping flag
            _isJumping = true;

        }

        // Landed
        else if (_isJumping && _isFalling && (_frontWheelCollision.isGrounded || _backWheelCollision.isGrounded))
        {
            // Reset jumping flags
            _isJumping = false;
            _isFalling = false;
        }
    }
}
