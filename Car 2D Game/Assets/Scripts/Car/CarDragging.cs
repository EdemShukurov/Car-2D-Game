using System;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Drag a Rigidbody2D by selecting one of its colliders by pressing the mouse/touch down.
/// When the collider is selected, add a TargetJoint2D.
/// While the mouse/finger is moving, continually set the target to the mouse/finger position.
/// When the mouse/touch is released, the TargetJoint2D is deleted.`
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class CarDragging : CarBaseMovement, IVehicleDraggingViaTargetJoint2D
{
    [Header("Wheels")]
    [SerializeField] private Rigidbody2D _frontWheelRigidBody2D;
    [SerializeField] private Rigidbody2D _backWheelRigidBody2D;

    [Header("SmokeActivity")]
    [SerializeField] private SmokeActivity _smokeActivity;

    [Header("TargetJoint2D properties")]

    [Range(0.0f, 100.0f)]
    [SerializeField] private float _damping = .8f;

    [Range(0.0f, 100.0f)]
    [SerializeField] private float _frequency = 5.0f;

    [Range(0.0f, 5000.0f)]
    [SerializeField] private float _maxForce = 1000.0f;

    private Vector2 _currentWorldPosition;

    private TargetJoint2D _targetJoint2D;

    private static IPivotRotation _pivotRotation;

    private float _originalCarMass, _originalWheelMass;

    public bool useDrag;

    private void OnEnable()
    {
        // Subcribe to events when object is enabled
        TouchManager.OnTouchDown += OnTouchDown;
        TouchManager.OnTouchUp += OnTouchUp;
        //TouchManager.OnTouchStationaryOrMoved += OnTouchStationaryOrMoved;
    }

    private void Start()
    {
        _pivotRotation = GetComponent<CarRotation>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
       
        _originalCarMass = _rigidBody2D.mass;
        _originalWheelMass = _backWheelRigidBody2D.mass;
    }


    #region OnTouchDown and OnMouseDown

    private void OnTouchDown(Touch eventData)
    {
        // get world touch position
        _currentWorldPosition = Camera.main.ScreenToWorldPoint(eventData.position);

        // Fetch the first collider.
        var collider = Physics2D.OverlapPoint(eventData.position);

        if (collider == null || collider.gameObject.tag != "Car")
        {
            // not drag, do car base movement
            _movementInput = base.GetTouch(eventData.position.x);
            return;
        }

        BeginVehicleDragging();
    }

    private void OnMouseDown()
    {
        // get world mouse click position
        _currentWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        BeginVehicleDragging();
    }

    #endregion

    public void BeginVehicleDragging()
    {
        _pivotRotation.AlignAngleCarViaPivot();

        // set flag
        useDrag = true;

        // off smoke while dragging
        _smokeActivity.enabled = false;

        AddTargetJoint2D();
        ChangeRigidBody2DProperties();
    }

    /// <summary>
    /// Add TargetJoint2D to Car
    /// </summary>
    private void AddTargetJoint2D()
    {
        // Add a target joint to the Rigidbody2D GameObject.
        _targetJoint2D = _rigidBody2D.gameObject.AddComponent<TargetJoint2D>();
        _targetJoint2D.dampingRatio = _damping;
        _targetJoint2D.frequency = _frequency;
        _targetJoint2D.maxForce = _maxForce;

        AddAnchorToTargetJoint2D();

    }

    /// <summary>
    /// Cancel the car rotation and decrease it's mass in order to the car will become easier
    /// </summary>
    private void ChangeRigidBody2DProperties()
    {
        _rigidBody2D.freezeRotation = true;
        _rigidBody2D.mass = 10f;

        if (_frontWheelRigidBody2D != null)
            _frontWheelRigidBody2D.mass = 40f;

        if (_backWheelRigidBody2D != null)
            _backWheelRigidBody2D.mass = 10f;
    }

    /// <summary>
    /// Attach the anchor to the local-point where we clicked
    /// </summary>
    private void AddAnchorToTargetJoint2D()
    {
        _targetJoint2D.anchor = _targetJoint2D.transform.InverseTransformPoint(_currentWorldPosition);
    }

    protected override void Update()
    {
        //Debug.Log("overr");
        if (useDrag == true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                EndVehicleDragging();
                return;
            }

            ClampVelocity();
            GetTouchPosition();
            AddTargetToTargetJoint2D();
        }
        // car base movement
        else
            base.Update();
    }

    private void ClampVelocity()
    {
        _rigidBody2D.velocity = new Vector2( Mathf.Clamp(_rigidBody2D.velocity.x, -20f, 20f), Mathf.Clamp(_rigidBody2D.velocity.y, -10f, 10f));
    }

    /// <summary>
    /// Refresh target position
    /// </summary>
    private void GetTouchPosition()
    {
        if (Input.touchCount > 0)
            _currentWorldPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        else
            _currentWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void AddTargetToTargetJoint2D()
    {
        if(_targetJoint2D != null)
            _targetJoint2D.target = _currentWorldPosition;
    }

    private void OnTouchUp(Touch eventData)
    {
        if (useDrag == true)
            EndVehicleDragging();
    }

    public void EndVehicleDragging()
    {
        //set flag
        useDrag = false;

        // on smoke
        _smokeActivity.enabled = true;

        DestroyTargetJoint2D();
        RefreshRigidBody2DProperties();
        _pivotRotation.DetachPivot();
    }

    /// <summary>
    /// Return car properties
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void RefreshRigidBody2DProperties()
    {
        _rigidBody2D.freezeRotation = false;
        _rigidBody2D.mass = _originalCarMass;

        _backWheelRigidBody2D.mass = _frontWheelRigidBody2D.mass = _originalWheelMass;
    }


    /// <summary>
    /// Remove TargetJoint2D
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DestroyTargetJoint2D()
    {
        Destroy(_targetJoint2D);
        _targetJoint2D = null;
    }

    private void OnDisable()
    {
        // Unsubcribe from events when object is disabled
        TouchManager.OnTouchDown -= OnTouchDown;
        TouchManager.OnTouchUp -= OnTouchUp;
        //TouchManager.OnTouchStationaryOrMoved -= OnTouchStationaryOrMoved;
    }
}