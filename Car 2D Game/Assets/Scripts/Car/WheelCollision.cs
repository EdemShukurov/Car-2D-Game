using UnityEngine;

public class WheelCollision : MonoBehaviour
{
    public const string TAG = "Ground";
    public bool isGrounded = false;

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TAG))
            isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TAG))
            isGrounded = true;
    }
}