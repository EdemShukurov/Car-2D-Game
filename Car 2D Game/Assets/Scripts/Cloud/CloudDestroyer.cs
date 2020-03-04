using UnityEngine;
using System.Collections;

public class CloudDestroyer : MonoBehaviour
{
    public const string TAG = "Cloud";

    private ObjectPooler _objectPooler;

    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == TAG)
        {
            collision.gameObject.SetActive(false);

            _objectPooler.EnqueeObject(TAG, collision.gameObject);
        }
    }
}
