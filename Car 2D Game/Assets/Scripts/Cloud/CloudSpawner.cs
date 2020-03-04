using UnityEngine;
using System.Collections;

public class CloudSpawner : MonoBehaviour
{
    private ObjectPooler _objectPooler;

    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
    }

    private void FixedUpdate()
    {
        ObjectPooler.Instance.SpawnFromPool("Cloud", transform.position, Quaternion.identity);
    }
}
