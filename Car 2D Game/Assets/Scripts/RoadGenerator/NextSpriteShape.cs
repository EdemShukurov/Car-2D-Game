using UnityEngine;

public class NextSpriteShape: MonoBehaviour
{
    [HideInInspector]
    public bool IsAction { get; set; }

    public SpriteShapeDisplayer spriteShapeDisplayer;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out CarTrigger carTrigger))
        {
            spriteShapeDisplayer.Display(transform.parent, true);
        }
    }
}
