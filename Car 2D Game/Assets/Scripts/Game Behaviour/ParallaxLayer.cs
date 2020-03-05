using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public GameObject cam;
    public float parallaxEffect;

    private float _length;
    private float _startPos;
    private float _dist;
    private float _temp;
    private float _offsetY;

    void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
        _offsetY = 10f;
    }

    private void Update()
    {
        _dist = cam.transform.position.x * parallaxEffect;
        _temp = cam.transform.position.x * (1f - parallaxEffect);

        transform.position = new Vector3(_startPos + _dist,
                                        cam.transform.position.y + _offsetY,
                                        transform.position.z);

        if (_temp > _startPos + _length)
        {
            _startPos += _length;
        }
        else if (_temp < _startPos - _length)
        {
            _startPos -= _length;
        }
            
    }
}
