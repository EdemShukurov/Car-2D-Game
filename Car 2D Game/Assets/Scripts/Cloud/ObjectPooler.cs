using UnityEngine;
using System.Collections.Generic;
using System;

public class ObjectPooler : MonoBehaviour
{
    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
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

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            var objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation)
    {
        if (poolDictionary.ContainsKey(tag) == false)
        {
            Debug.LogWarning("Pool with tag " + tag + " doesnt exist");
            return null;
        }

        if (poolDictionary[tag].Count < 0)
        {
            Debug.LogWarning("Queue is epmty, tag: " + tag);
            return null;
        }
           
       
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if(pooledObject != null)
        {
            pooledObject.OnObjectSpawned();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
