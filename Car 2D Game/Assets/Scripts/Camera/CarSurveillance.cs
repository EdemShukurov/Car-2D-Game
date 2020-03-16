using UnityEngine;

public class CarSurveillance : MonoBehaviour
{
    public Transform target;

    private Vector3 offset;

    private void Awake() => offset = transform.position - target.position;

    private void Update() => transform.position = offset + target.position;

}
