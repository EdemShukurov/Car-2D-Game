using UnityEngine;

public class WheelCollision : MonoBehaviour
{
    public bool isGrounded = false;

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
            isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
            isGrounded = true;
    }
}