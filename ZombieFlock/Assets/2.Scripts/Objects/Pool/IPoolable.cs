using UnityEngine;

/// <summary>
/// @tk : Pool 사용할 객체들은 해당 인터페이스 상속 필요
/// </summary>
public interface IPoolable 
{
    public string PoolPath { get; }
    public GameObject Prefab { get; }
}
