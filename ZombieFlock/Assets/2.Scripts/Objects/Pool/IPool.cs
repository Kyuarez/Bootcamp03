using System.Collections.Generic;
using UnityEngine;

public interface IPool
{
    public IPoolable Model { get; set; }
    public Transform ObjParent { get; set; }
    public int PoolCount { get; }
    public int MaxCount { get; set; }

    public void InitPool(GameObject prefab, Transform objParent, int maxCount);
    public GameObject SpawnObject(Transform parent);
    public void DeSpawnObject(GameObject prefab);
}
