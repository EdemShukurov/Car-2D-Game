using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.RoadGenerator;
using System.Runtime.CompilerServices;


public class SpriteShapeDisplayer : MonoBehaviour
{
    public int countOfGround;

    public Transform finalGround;

    public GroundPool GroundPool { get; set; }

    private static float _offsetXPosition = 200f, _offsetXPositionIncrement = 200f;

    private int _lastIndex, _secondLastIndex;

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        SetFinalGround();

        GroundPool = GroundPool.GetInstance();
        
        List<int> list = new List<int>();
        int countOfChilds = transform.childCount;

        SetTheFirstTwoRandomGround(ref list, countOfChilds);
       
        do
        {
            int index = Random.Range(0, countOfChilds);

            if (_lastIndex == index || _secondLastIndex == index)
            {
                continue;
            }

            list.Add(index);
            _secondLastIndex = _lastIndex;
            _lastIndex = index;
        }
        while (list.Count < countOfGround);

        AddAllElementsToGroundPool(ref list);

        DisplayTheFirstGround();
    }

    private void AddAllElementsToGroundPool(ref List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GroundPool.AddLast(transform.GetChild(list[i]));
        }
    }

    private void SetTheFirstTwoRandomGround(ref List<int> list, int count)
    {
        int firstIndex = Random.Range(0, count);
        list.Add(firstIndex);

        int secondIndex = Random.Range(0, count);
       
        while(list.Contains(secondIndex) == false)
        {
            list.Add(secondIndex);
        }

        _secondLastIndex = firstIndex;
        _lastIndex = secondIndex;
    }

    private void SetFinalGround()
    {
        if (finalGround != null)
            finalGround.position = new Vector3(_offsetXPosition + countOfGround * _offsetXPositionIncrement, 0f, 0f);
    }

    private void DisplayTheFirstGround() => ShowGround(GroundPool.GetFirst());

    public void Display(Transform current, bool next = true)
    {
        float currentXPosition = current.position.x;

        var nextGround = GroundPool.GetNextFromCurrent(currentXPosition);
        var previousGround = GroundPool.GetPreviousFromCurrent(currentXPosition);

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
    private void ShowGround(Transform ground)
    {
        if(ground != null)
            ground.gameObject.SetActive(true);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void HideGround(Transform ground)
    {
        if (ground != null)
            ground.gameObject.SetActive(false);
    }
}

