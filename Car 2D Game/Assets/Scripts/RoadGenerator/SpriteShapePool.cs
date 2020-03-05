using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Linq;
using System;

public class SpriteShapePool
{
    public CircularDoublyLinkedList<Transform> SpriteShapeControllers = null;

    public SpriteShapePool()
    {
        SpriteShapeControllers = new CircularDoublyLinkedList<Transform>();
        //InitListOfSpriteShapes();
    }

    public void Add(Transform spriteShapeController)
    {
        SpriteShapeControllers.Add(spriteShapeController);
    }

    public void Remove(Transform spriteShapeController)
    {
        if (SpriteShapeControllers.Remove(spriteShapeController))
            return;
        else
            throw new ArgumentException();        
    }

    public List<Transform> GetSpritesFromCurrent(Transform spriteShapeController, int count = 3)
    {
        return SpriteShapeControllers.GetElements(spriteShapeController, count);
    }

    public void SetActiveElement(Transform spriteShapeController)
    {
        SpriteShapeControllers.SetActiveElement(spriteShapeController);
        SetActionSpriteShapeScript(spriteShapeController);       
    }

    private void SetActionSpriteShapeScript(Transform spriteShapeController)
    {
        foreach (var item in SpriteShapeControllers)
        {
            if(item != spriteShapeController)
                item.GetComponent<SpriteShapeAction>().IsAction = false;
            else
                item.GetComponent<SpriteShapeAction>().IsAction = true;
        }
    }

    public Transform GetActionSpriteShapeScript()
    {
        foreach (var item in SpriteShapeControllers)
        {
            if (item.GetComponent<SpriteShapeAction>().IsAction == true)
                return item;
        }
        return null;
    }

    public Transform GetActiveElement()
    {
        return SpriteShapeControllers.GetActiveElement();
    }

    public Transform GetPreviousElementFromActiveElemenet()
    {
        return SpriteShapeControllers.GetPreviousElementFromActiveElemenet();
    }

    public Transform GetNextElementFromActiveElemenet()
    {
        return SpriteShapeControllers.GetNextElementFromActiveElemenet();
    }

    public List<Transform> GetRequiredListOfSpriteShapes()
    {
       return SpriteShapeControllers.GetRequiredList();
        //RemoveUnnecessarySpriteShapes(list);
    }

    public Transform GetCurrentActiveElement()
    {
        return SpriteShapeControllers.GetActiveElement();
    }

    public List<Transform> GetAllElements()
    {
        return SpriteShapeControllers.GetAllElements();
    }
}