using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour, IPooledObject
{
    public float upSide = 30f;
    public float downSide = 0f;

    //public GameObject cloudSpawner;

    public void OnObjectSpawned()
    {
        float yPosition = Random.Range(downSide, upSide);

        Vector2 force = new Vector2(0f, yPosition);

        GetComponent<Rigidbody2D>().velocity = force;
    }
}
