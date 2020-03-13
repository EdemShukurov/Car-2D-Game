using UnityEngine;

public class Cloud : MonoBehaviour, IPooledObject
{
    private float _upSide;
    private float _downSide;

    private float _speed;
    private float _xPosition;

    private CloudSpawner _cloudSpawner;


    private void OnEnable()
    {
        _cloudSpawner = CloudSpawner.Instance;

        // set initial x-coordinate, it depends on cloudSpawner 
        _xPosition = _cloudSpawner.transform.position.x;

        var boxCollider = _cloudSpawner.GetComponent<BoxCollider2D>();
        
        if (boxCollider != null)
        {
            _upSide = boxCollider.size.x;
            _downSide = boxCollider.size.y;
        }
    }

    /// <summary>
    /// Cloud class implements interface, we set a cloud in random position for Y-coordinate and set a speed
    /// </summary>
    public void OnObjectSpawned()
    {
        float yPosition = Random.Range(_downSide, _upSide);
        transform.position = new Vector2(_xPosition, yPosition);

        _speed = Random.Range(3f, 3.5f);
    }

    /// <summary>
    /// After a cloud was appeared, it should move smoothly in the left side
    /// </summary>
    private void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.smoothDeltaTime, Space.World);
    }
}
