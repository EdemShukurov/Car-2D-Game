using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeFalling : MonoBehaviour
{
    private const string tag = "Wheel";
    public GameObject baseBridge;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tag))
        {
            baseBridge.SetActive(false);
        }
    }
}
