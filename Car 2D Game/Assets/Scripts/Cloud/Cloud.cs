using UnityEngine;

public class Cloud : MonoBehaviour, IPooledObject
{
    private float _upSide;
    private float _downSide;

    private float _speed;

    private float _xPosition;

    private CloudSpawner _cloudSpawner;

    private void Start()
    {
        _cloudSpawner = CloudSpawner.Instance;
        _xPosition = _cloudSpawner.transform.position.x;

        var boxCollider = _cloudSpawner.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            _upSide = boxCollider.size.x;
            _downSide = boxCollider.size.y;
        }
    }

    public void OnObjectSpawned()
    {
        float yPosition = Random.Range(_downSide, _upSide);
        transform.position = new Vector2(_xPosition, yPosition);

        _speed = Random.Range(3f, 5f);
    }

    private void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.smoothDeltaTime, Space.World);
    }
}
