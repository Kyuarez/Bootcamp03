using System;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private int codeID;
    [SerializeField] private int spawnObjectID; //@tk 지금은 좀비로 고정(25.03.26)
    
    private Vector3 spawnPos;
    private SphereCollider col;
    private GameObject spawnObj;

    public event Action OnPostSpawn;

    public int CodeID => codeID;


    public void InitSpawnTrigger()
    {
        //spawnPos가져오기 (world인지 체크 : 월드 좌표)
        spawnPos = transform.GetChild(0).transform.position;

        col = GetComponent<SphereCollider>();//trigger
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") == true)
        {
            spawnObj = PoolManager.Instance.SpawnObjectInWorld<ZombieManager>(spawnPos);
            col.enabled = false;
            OnPostSpawn?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        float radius = col.radius;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
