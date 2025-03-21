using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

//@tk : 
public class PoolManager : MonoSingleton<PoolManager>
{
    [SerializeField] private int poolMaxCount = 20;

    //GameObject에 클래스하고 IPoolable 해서 관리하기
    private Dictionary<string, IPool> poolDict = new Dictionary<string, IPool>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //@tk : 생성은 start에 하는 게 좋을 듯. (awake는 캐싱 및 데이터 생성)
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
        if (poolDict.ContainsKey(pool.Model.PoolPath) == true)
        {
            return;
        }

        poolDict.Add(pool.Model.PoolPath, pool);
    }

    public GameObject SpawnObject(string path, Transform Root = null)
    {
        if (poolDict.ContainsKey(path) == false)
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
    public GameObject SpawnObjectInWorld(string path, Vector3 position)
    {
        if (poolDict.ContainsKey(path) == false)
        {
            return null;
        }

        var pool = poolDict[path];
        return pool.SpawnObjectInWorld(position);
    }
    public GameObject SpawnObjectInWorld<T>(Vector3 position)
    {
        if (poolDict.ContainsKey(typeof(T).Name) == false)
        {
            return null;
        }

        var pool = poolDict[typeof(T).Name];
        return pool.SpawnObjectInWorld(position);
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
        if (poolDict.ContainsKey(poolObj.PoolPath) == false)
        {
            Destroy(poolObj.Prefab);
            return;
        }

        var pool = poolDict[poolObj.PoolPath];
        pool.DeSpawnObject(poolObj.Prefab);
    }
    public void DeSpawnObjectDelay(IPoolable poolObj)
    {
        if (poolDict.ContainsKey(poolObj.PoolPath) == false)
        {
            Destroy(poolObj.Prefab);
            return;
        }

        var pool = poolDict[poolObj.PoolPath];
        pool.DeSpawnObject(poolObj.Prefab);
    }

    public bool IsExistPool(string poolObject)
    {
        return poolDict.ContainsKey(poolObject);
    }
}
