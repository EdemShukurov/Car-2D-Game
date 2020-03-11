using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.RoadGenerator
{
    public class GroundPool
    {
        LinkedList<Transform> transforms = new LinkedList<Transform>();

        private static float _offsetXPosition = 200f;
        private static float _offsetXPositionIncrement = 200f;

        private static GroundPool instance;

        private GroundPool()
        { }

        public static GroundPool GetInstance()
        {
            if (instance == null)
                instance = new GroundPool();

            return instance;
        }

        public void AddLast(Transform transform, bool setOffset = true)
        {
            if(setOffset)
            {
                // set offset for every ground
                transform.position = new Vector3(_offsetXPosition, 0f, 0f);

                // increase offset
                _offsetXPosition += _offsetXPositionIncrement;
            }

            // add to linkedlist
            transforms.AddLast(transform);
        }

        public Transform GetFirst() => transforms.First.Value;
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LinkedListNode<Transform> GetCurrentNode(Transform current) => transforms.Find(current);


        public Transform GetNextFromCurrent(Transform current)
        {
            // Find the current node
            var currentNode = GetCurrentNode(current);

            if (currentNode.Next == null)
                return null;

            return currentNode.Next.Value;
        }

        public Transform GetPreviousFromCurrent(Transform current)
        {
            // Find the current node
            var currentNode = GetCurrentNode(current);

            if (currentNode.Previous == null)
                return null;

            return currentNode.Previous.Value;
        }           
            
    }
}
