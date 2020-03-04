using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(Vector3.left * 5f * Time.deltaTime, Space.World);
    }
}
