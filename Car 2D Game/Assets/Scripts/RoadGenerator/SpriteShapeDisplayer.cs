using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.RoadGenerator;
using System.Runtime.CompilerServices;

public class SpriteShapeDisplayer : MonoBehaviour
{
    public GroundPool GroundPool { get; set; }

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        GroundPool = GroundPool.GetInstance();

        const int countOfGround = 3;
        int countOfChilds = transform.childCount;
        int index = Random.Range(0, countOfChilds);

        List<int> list = new List<int>();
        
        list.Add(index);

        do
        {
            index = Random.Range(0, countOfChilds);

            if (list.Contains(index) == false)
                list.Add(index);
        }
        while (list.Count < countOfGround);


        for (int i = 0; i < list.Count; i++)
        {
           GroundPool.AddLast(transform.GetChild(list[i]));
        }

        DisplayTheFirstGround();
    }

    private void DisplayTheFirstGround() => ShowGround(GroundPool.GetFirst());


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ShowGround(Transform ground)
    {
        if(ground != null)
            ground.gameObject.SetActive(true);
    }


    public void Display(Transform current, bool next = true)
    {
        var nextGround = GroundPool.GetNextFromCurrent(current);
        var previousGround = GroundPool.GetPreviousFromCurrent(current);

        if (next == true)
        {
            ShowGround(nextGround);
            HideGround(previousGround);
        }
        else
        {
            HideGround(nextGround);
            ShowGround(previousGround);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void HideGround(Transform ground)
    {
        if (ground != null)
            ground.gameObject.SetActive(false);
    }
}

