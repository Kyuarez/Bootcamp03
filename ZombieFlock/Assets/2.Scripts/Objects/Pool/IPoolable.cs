using UnityEngine;

/// <summary>
/// @tk : Pool ����� ��ü���� �ش� �������̽� ��� �ʿ�
/// </summary>
public interface IPoolable 
{
    public string PoolPath { get; }
    public GameObject Prefab { get; }
}
