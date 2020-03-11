using UnityEngine;

public class FinishFlag : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<CarTrigger>(out CarTrigger carTrigger))
        {
            carTrigger.SmoothBrakeCar();
        }
    }
}
