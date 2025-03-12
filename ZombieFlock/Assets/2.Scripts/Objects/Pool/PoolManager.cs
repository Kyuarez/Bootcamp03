using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//@tk : 
public class PoolManager : MonoSingleton<PoolManager>
{
    [SerializeField] private int poolMaxCount = 20;

    private Dictionary<string, IPool> poolDict = new Dictionary<string, IPool>();

    protected override void Awake()
    {
        Init();
    }

    public void Init()
    {
        //Resources에서 IPoolable인거 가져오기
        GameObject[] poolObjs = Resources.LoadAll<GameObject>("")
            .Where(obj => obj.GetComponent<IPoolable>() != null)
            .ToArray();

        foreach (var poolObj in poolObjs)
        {
            IPoolable poolable = poolObj.GetComponent<IPoolable>();
            GameObject objParent = new GameObject($"Pool:{poolable.PoolPath}");
            objParent.transform.parent = transform;

            ObjectPool pool = new ObjectPool();
            pool.InitPool(poolObj, objParent.transform, poolMaxCount);
            AddPool(pool);
        }
    }

    private void AddPool(IPool pool)
    {
        if(poolDict.ContainsKey(pool.Model.PoolPath) == true)
        {
            return;
        }

        poolDict.Add(pool.Model.PoolPath, pool);
    }

    public GameObject SpawnObject(string path, Transform Root = null)
    {
        if(poolDict.ContainsKey(path) == false)
        {
            return null;
        }

        var pool = poolDict[path];
        return pool.SpawnObject(Root);
    }
    public GameObject SpawnObject<T>(Transform Root = null) where T : MonoBehaviour
    {
        if (poolDict.ContainsKey(typeof(T).Name) == false)
        {
            return null;
        }

        var pool = poolDict[typeof(T).Name];
        return pool.SpawnObject(Root);
    }
    public GameObject SpawnObject<T>(Transform Root, Vector3 localPosition) where T : MonoBehaviour
    {
        if (poolDict.ContainsKey(typeof(T).Name) == false)
        {
            return null;
        }

        var pool = poolDict[typeof(T).Name];
        return pool.SpawnObject(Root, localPosition);
    }

    public void DeSpawnObject(IPoolable poolObj)
    {
        if(poolDict.ContainsKey(poolObj.PoolPath) == false)
        {
            Destroy(poolObj.Prefab);
            return;
        }

        var pool = poolDict[poolObj.PoolPath];
        pool.DeSpawnObject(poolObj.Prefab);
    }
}
