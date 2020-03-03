using UnityEngine;
using System.Collections;

public class CarRotation : MonoBehaviour, IPivotRotation, IAngleCarRotation
{
    [Header("Pivot")]
    public GameObject pivot;

    [Header("Wheels")]
    public WheelCollision _frontWheelCollision;
    public WheelCollision _backWheelCollision;

    [SerializeField] private float _speedChangeAngle = 0.8f;

    // fields for angle limit
    private float _minimumZ = -20F;
    private float _maximumZ = 20F;


    /// <summary>
    /// Align Angle Car Via Pivot during the first touch/mouse click on the car
    /// </summary>
    public void AlignAngleCarViaPivot()
    {
        if (transform.eulerAngles.z == 0f)
            return;

        Quaternion targetAngle;

        if (transform.eulerAngles.z > 0f)
        {
            SetPivot(_backWheelCollision.transform.position);
            targetAngle = Quaternion.Euler(0f, 0f, -transform.eulerAngles.z);
            StartCoroutine(RotatePivot(targetAngle, _speedChangeAngle));
        }
        else
        {
            SetPivot(_frontWheelCollision.transform.position);
            targetAngle = Quaternion.Euler(0f, 0f, transform.eulerAngles.z);
            StartCoroutine(RotatePivot(targetAngle, _speedChangeAngle));
        }

        //DetachPivot();
    }

    /// <summary>
    /// Set pivot to define wheel position and set it as a parent "Car" GameObject (in scene: Car -> CarBase)
    /// </summary>
    /// <param name="wheelPosition">wheel position</param>
    private void SetPivot(Vector3 wheelPosition)
    {
        pivot.transform.position = wheelPosition;
        transform.parent = pivot.transform;
    }

    /// <summary>
    /// Set off parent object
    /// </summary>
    public void DetachPivot()
    {
        // refresh pivot (parent object) 
        transform.parent = null;
        pivot.transform.localEulerAngles = Vector3.zero;
    }

    /// <summary>
    /// Use Coroutine in order to rotate pivot smoothly
    /// </summary>
    /// <param name="targetAngle"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator RotatePivot(Quaternion targetAngle, float time)
    {
        var originalTime = time;
        Quaternion startAngle = pivot.transform.rotation;

        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            pivot.transform.rotation = Quaternion.Lerp(startAngle, targetAngle, 1 - time / originalTime);
            yield return null;
        }
    }

    /// <summary>
    /// Limit car angle in order to avoid car flip
    /// </summary>
    public void LimitAngleCar()
    {
        if (_frontWheelCollision.isGrounded == false || _backWheelCollision.isGrounded == false)
        {
            var angleCar = ClampAngleCar(transform.eulerAngles.z, _minimumZ, _maximumZ);
            transform.rotation = RotateAngleCar(angleCar);
        }
    }

    /// <summary>
    /// Clamp car angle
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns>permissible car angle</returns>
    private float ClampAngleCar(float angle, float min, float max)
    {
        if (angle < -360F) angle += 360F;

        //if (angle > 360F) angle -= 360F;

        if (angle >= 180f) angle -= 360f;

        return Mathf.Clamp(angle, min, max);
    }


    private Quaternion RotateAngleCar(float angle)
    {
        return Quaternion.Euler(0f, 0f, angle);
    }



}