using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] protected float spawnRadius = 30.0f;
    [SerializeField] protected int spawnMaxCount = 20;
    [SerializeField] protected float spawnDelay = 2.0f;

    protected float elapsedTime = 0.0f;
    protected Queue<GameObject> zombieQueue;

    private void Awake()
    {
        zombieQueue = new Queue<GameObject>();
    }

    private void Update()
    {
        if(zombieQueue.Count >= spawnMaxCount)
        {
            return;
        }

        elapsedTime += Time.time;

        if(elapsedTime > spawnDelay)
        {
            //Spawn
            Vector3 randomPos = GetRandomPointInCircle();
            GameObject zombie = PoolManager.Instance.SpawnObject<ZombieManager>(transform, randomPos);
            zombieQueue.Enqueue(zombie);
            elapsedTime = 0.0f;
        }
    }

    Vector3 GetRandomPointInCircle()
    {
        float randomRadius = Mathf.Sqrt(Random.value) * spawnRadius; // 균등 분포를 위한 sqrt
        float randomAngle = Random.Range(0, 2 * Mathf.PI); // 0 ~ 360도 랜덤

        float x = randomRadius * Mathf.Cos(randomAngle);
        float z = randomRadius * Mathf.Sin(randomAngle);
        float y = 0; // 고정

        return new Vector3(x, y, z) + transform.position; // 현재 Transform 위치 기준
    }


    //@tk : 스폰 영역
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
