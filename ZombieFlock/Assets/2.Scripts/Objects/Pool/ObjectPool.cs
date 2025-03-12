using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPool : IPool
{
    protected Queue<GameObject> pools = new Queue<GameObject>();
    public IPoolable Model { get; set; }
    public Transform ObjParent { get; set; }
    public int PoolCount { get; }
    public int MaxCount { get; set; }

    public void InitPool(GameObject prefab, Transform objParent, int maxCount)
    {
        Model = prefab.GetComponent<IPoolable>();
        ObjParent = objParent;
        MaxCount = maxCount;

        for (int i = 0; i < maxCount; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab);
            pools.Enqueue(obj);
            obj.transform.parent = objParent;
            obj.SetActive(false);
        }
    }
    public GameObject SpawnObject(Transform parent)
    {
        if(pools.Count == 0)
        {
            return null;
        }

        GameObject obj = pools.Dequeue();
        obj.SetActive(true);
        obj.transform.parent = parent;
        return obj;
    }
    public GameObject SpawnObject(Transform parent, Vector3 localPosition)
    {
        if (pools.Count == 0)
        {
            return null;
        }

        GameObject obj = pools.Dequeue();
        obj.SetActive(true);
        obj.transform.parent = parent;
        obj.transform.position = localPosition;
        return obj;
    }

    public void DeSpawnObject(GameObject prefab)
    {
        prefab.gameObject.SetActive(false);
        prefab.transform.parent = ObjParent; 
        pools.Enqueue(prefab);
    }
}
