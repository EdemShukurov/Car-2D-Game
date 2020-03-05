using UnityEngine;
using System.Collections;

public class CloudSpawner : MonoBehaviour
{
    public const string TAG = "Cloud";

    private float _repeatRate = 3.4f;
    private float _startTime = 0.5f;

    private ObjectPooler _objectPooler;

    #region Singleton

    public static CloudSpawner Instance;

    private void Awake()
    {
        Instance = this;

        Time.timeScale = 5.8f;
        Time.fixedDeltaTime *= Time.timeScale;
    }

    #endregion

    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
        InvokeRepeating("Spawn", _startTime, _repeatRate);
    }

    private void Spawn()
    {
        _objectPooler.SpawnFromPool(TAG, transform.position, Quaternion.identity);
    }


}
