using UnityEngine;

public class FX_RiflingMark_Concrete : MonoBehaviour, IPoolable
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
}
