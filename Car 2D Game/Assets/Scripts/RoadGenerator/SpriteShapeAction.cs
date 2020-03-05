using System;
using UnityEngine;

public class SpriteShapeAction: MonoBehaviour
{
    public bool IsAction { get; set; }

    public SpriteShapeDisplayer spriteShapeDisplayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Car")
        {
            spriteShapeDisplayer.SetActiveSpriteShape(transform.parent);
        }
    }
}
