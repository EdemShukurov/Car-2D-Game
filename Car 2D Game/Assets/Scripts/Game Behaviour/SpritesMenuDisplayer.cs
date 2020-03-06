using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpritesMenuDisplayer : MonoBehaviour
{
    public const string TAG = "Menu Image";

    private float _offset = 130f;
    private float startPosition = 256f;
    private float endPosition = -263f;

    private ObjectPooler _objectPooler;


    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
    }

    private void Spawn()
    {
    }

}
