using UnityEngine;
using System.Collections;

public class CarSurveillance : MonoBehaviour
{
    public Transform target;

    public float restriction = 35f;

    private Vector3 offset;

    private void Awake()
    {
        offset = transform.position - target.position;
    }

    private void Update()
    {
        if (transform.position.y < restriction && 
            offset.y + target.position.y < restriction)
        {
            transform.position = offset + target.position;
        }
    }

}
