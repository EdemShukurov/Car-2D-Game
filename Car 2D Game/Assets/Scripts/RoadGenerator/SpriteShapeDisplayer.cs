using UnityEngine;
using System.Collections;
using UnityEngine.U2D;
using System;
using System.Collections.Generic;

public class SpriteShapeDisplayer : MonoBehaviour
{
    public Transform carPosition;

   // public List<PrefabRule> PrefabRules;

   // public PrefabManager prefabManager;

    private LayerMask _mask;
    private static float offset = 200f;

    public SpriteShapePool SpriteShapePool { get; set; }

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        _mask = LayerMask.GetMask("Ground");

        SpriteShapePool = new SpriteShapePool();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            SpriteShapePool.SpriteShapeControllers.Add(transform.GetChild(i));
        }

        //prefabManager = new PrefabManager(SpriteShapePool, PrefabRules);
    }

    public void SetActiveSpriteShape(Transform spriteShapeAction)
    {
        SpriteShapePool.SetActiveElement(spriteShapeAction);
        Display();
    }

    private void Display()
    {
        var previousSpriteShape = SpriteShapePool.GetPreviousElementFromActiveElemenet();
        var nextSpriteShape = SpriteShapePool.GetNextElementFromActiveElemenet();     
        var activeSpriteShape = SpriteShapePool.GetActiveElement();
     
        
        //PrefabPool.
        nextSpriteShape.position = new Vector3(activeSpriteShape.position.x + offset, nextSpriteShape.position.y, 0f);

        previousSpriteShape.position = new Vector3(activeSpriteShape.position.x - offset, nextSpriteShape.position.y, 0f);
    }

}
