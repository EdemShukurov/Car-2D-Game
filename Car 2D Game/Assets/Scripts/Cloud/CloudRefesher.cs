using UnityEngine;

public class CloudRefesher : MonoBehaviour
{
    public const string TAG = "Cloud";

    private ObjectPooler _objectPooler;


    #region Singleton

    public static CloudRefesher Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(TAG))
        {
            collision.gameObject.SetActive(false);

            _objectPooler.EnqueeObject(TAG, collision.gameObject);
        }
    }
}
