using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.RoadGenerator
{
    public class GroundPool
    {
        Dictionary<float, Transform> grounds = new Dictionary<float, Transform>();

        private static float _offsetXPosition = 200f, _offsetXPositionIncrement = 200f;

        #region Singleton

        private static GroundPool instance;

        private GroundPool()
        { }

        public static GroundPool GetInstance()
        {
            if (instance == null)
                instance = new GroundPool();

            return instance;
        }

        #endregion

        public void AddLast(Transform transform)
        {
            grounds.Add(_offsetXPosition, transform);

            _offsetXPosition += _offsetXPositionIncrement;
        }

        public Transform GetFirst()
        {
            var ground = grounds[_offsetXPositionIncrement];
            ground.position = new Vector3(_offsetXPositionIncrement, 0f, 0f);

            return ground;
        }

        public Transform GetNextFromCurrent(float keyPosition)
        {
            keyPosition += _offsetXPositionIncrement;

            return GetElementByKey(keyPosition);
        }

        private Transform GetElementByKey(float key)
        {
            if (grounds.ContainsKey(key))
            {
                var result = grounds[key];
                result.position = new Vector3(key, 0f, 0f);
                return result;
            }

            return null;
        }

        public Transform GetPreviousFromCurrent(float keyPosition)
        {
            keyPosition -= _offsetXPositionIncrement;

            return GetElementByKey(keyPosition);
        }
    }
}
