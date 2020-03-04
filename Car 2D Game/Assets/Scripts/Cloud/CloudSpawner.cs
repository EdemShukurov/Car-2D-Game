using UnityEngine;
using System.Collections;

public class CloudSpawner : MonoBehaviour
{
    public const string TAG = "Cloud";

    #region Singleton

    public static CloudSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion


    private ObjectPooler _objectPooler;

    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
    }

    private void Update()
    {      
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(Random.Range(5f, 15f));
        _objectPooler.SpawnFromPool(TAG, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(5f);

    }



}
