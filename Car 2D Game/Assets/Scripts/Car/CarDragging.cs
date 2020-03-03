using UnityEngine;

/// <summary>
/// Drag a Rigidbody2D by selecting one of its colliders by pressing the mouse down.
/// When the collider is selected, add a TargetJoint2D.
/// While the mouse is moving, continually set the target to the mouse position.
/// When the mouse is released, the TargetJoint2D is deleted.`
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class CarDragging : CarBaseMovement, IVehicleDraggingViaTargetJoint2D
{
    [Header("TargetJoint2D properties")]

    [Range(0.0f, 100.0f)]
    [SerializeField] private float _damping = .8f;

    [Range(0.0f, 100.0f)]
    [SerializeField] private float _frequency = 5.0f;

    [Range(0.0f, 1000.0f)]
    [SerializeField] private float _maxForce = 25.0f;


    [SerializeField] private static float speedChangeAngle = 0.6f;

    private Vector2 _currentWorldPosition;
    private bool _useDrag;

    private Rigidbody2D _rigidbody;
    private TargetJoint2D _targetJoint2D;

    private static IPivotRotation _pivotRotation;

    private void OnEnable()
    {
        // Subcribe to events when object is enabled
        TouchManager.OnTouchDown += OnTouchDown;
        TouchManager.OnTouchUp += OnTouchUp;
        //TouchManager.OnTouchStationaryOrMoved += OnTouchStationaryOrMoved;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        AddTargetJoint2D();
        DisableTargetJoint2D();


        _pivotRotation = GetComponent<CarRotation>();
    }

    /// <summary>
    /// Add TargetJoint2D to Car
    /// </summary>
    private void AddTargetJoint2D()
    {
        // Add a target joint to the Rigidbody2D GameObject.
        _targetJoint2D = _rigidbody.gameObject.AddComponent<TargetJoint2D>();
        _targetJoint2D.dampingRatio = _damping;
        _targetJoint2D.frequency = _frequency;
        _targetJoint2D.maxForce = _maxForce;
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
            base.GetTouch(eventData.position.x);
            return;
        }

        BeginDragging();
    }

    private void OnMouseDown()
    {
        // get world mouse click position
        _currentWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        BeginDragging();
    }

    #endregion

    public void BeginDragging()
    {
        _pivotRotation.AlignAngleCarViaPivot();

        // set flag
        _useDrag = true;

        AddAnchorToTargetJoint2D();

        EnableTargetJoint2D();
    }


    public void AddAnchorToTargetJoint2D()
    {
        // Attach the anchor to the local-point where we clicked.
        _targetJoint2D.anchor = _targetJoint2D.transform.InverseTransformPoint(_currentWorldPosition);
    }


    public void EnableTargetJoint2D()
    {
        _targetJoint2D.enabled = true;
    }

    protected override void Update()
    {
        if (_useDrag == true)
        {
            GetTouchPosition();
            AddTargetToTargetJoint2D();
        }
        else if (Input.GetMouseButtonUp(0) && _useDrag)
        {
            EndDragging();
        }
        else
            // car base movement
            base.Update();
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
        _targetJoint2D.target = _currentWorldPosition;
    }

    //private void OnTouchStationaryOrMoved(Touch eventData)
    //{
    //    if(_useDrag == true)
    //        _currentWorldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
    //}

    private void OnTouchUp(Touch eventData)
    {
        if (_useDrag == true)
            EndDragging();
    }

    public void EndDragging()
    {
        //set flag
        _useDrag = false;

        DisableTargetJoint2D();
    }

    public void DisableTargetJoint2D()
    {
        _targetJoint2D.enabled = false;
    }

    private void OnDisable()
    {
        // Unsubcribe from events when object is disabled
        TouchManager.OnTouchDown -= OnTouchDown;
        TouchManager.OnTouchUp -= OnTouchUp;
        //TouchManager.OnTouchStationaryOrMoved -= OnTouchStationaryOrMoved;
    }
}