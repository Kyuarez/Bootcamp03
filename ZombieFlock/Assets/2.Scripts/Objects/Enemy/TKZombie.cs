using UnityEngine;

public class TKZombie : MonoBehaviour, IPoolable
{
    public string PoolPath 
    {
        get
        {
            return this.GetType().Name;
        }
    }

    public GameObject Prefab 
    {
        get
        {
            return gameObject;
        }
    }

    public void Dead()
    {
        PoolManager.Instance.DeSpawnObject(this);
    }
}
