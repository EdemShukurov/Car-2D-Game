using UnityEngine;
using System.Collections.Generic;
using System;

public class ObjectPooler : MonoBehaviour
{
    //[Serializable]
    //public class Pool
    //{
    //    public string tag;
    //    public GameObject prefab;
    //    public int size;
    //}

    [Serializable]
    public class Pool
    {
        public string tag;
        public List<GameObject> prefabs;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #region Singleton

    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    //private void Start()
    //{
    //    poolDictionary = new Dictionary<string, Queue<GameObject>>();

    //    foreach (Pool pool in pools)
    //    {
    //        var objectPool = new Queue<GameObject>();

    //        for (int i = 0; i < pool.size; i++)
    //        {
    //            var obj = Instantiate(pool.prefab);
    //            obj.SetActive(false);
    //            objectPool.Enqueue(obj);
    //        }

    //        poolDictionary.Add(pool.tag, objectPool);
    //    }
    //}

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            var objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.prefabs.Count; i++)
            {
                var obj = Instantiate(pool.prefabs[i]);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    /// <summary>
    /// Spawn from Pool
    /// </summary>
    /// <param name="tag">prefab type</param>
    /// <param name="position">where prefab should be placed</param>
    /// <param name="rotation">prefab's rotation</param>
    /// <returns></returns>
    public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation, Transform parent = null)
    {
        if (poolDictionary.ContainsKey(tag) == false)
        {
            Debug.LogWarning("Pool with tag " + tag + " doesnt exist");
            return null;
        }

        if (poolDictionary[tag].Count == 0)
        {
            Debug.LogWarning("Queue is epmty, tag: " + tag);
            return null;
        }
       
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        if(parent != null)
        {
            objectToSpawn.transform.parent = parent;
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if(pooledObject != null)
        {
            pooledObject.OnObjectSpawned();
        }

        //poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void EnqueeObject(string tag, GameObject objectToPull)
    {
        if (poolDictionary.ContainsKey(tag) == false)
        {
            Debug.LogWarning("Pool with tag " + tag + " doesnt exist");
            return;
        }

        poolDictionary[tag].Enqueue(objectToPull);
    }
}
