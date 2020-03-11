using UnityEngine;

public class SmokeActivity : MonoBehaviour
{
    [SerializeField] private CarBaseMovement _carBaseMovement;
    [SerializeField] private ParticleSystem _frontWheelSmoke;
    [SerializeField] private ParticleSystem _backWheelSmoke;
    
    private void OnEnable()
    {
        _carBaseMovement.OnSmokeSet += DisplaySmoke;
    }

    private void OnDisable()
    {
        _carBaseMovement.OnSmokeSet -= DisplaySmoke;
    }

    public void DisplaySmoke(bool isRight)
    {
        if (_carBaseMovement == null) return;

        if(isRight)
            _frontWheelSmoke.transform.rotation = _backWheelSmoke.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            _frontWheelSmoke.transform.rotation = _backWheelSmoke.transform.rotation = Quaternion.Euler(0f, 0f, 180f);

        _frontWheelSmoke.Play(true);
        _backWheelSmoke.Play(true);
    }
}
