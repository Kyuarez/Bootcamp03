using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private Transform plane1;
    [SerializeField] private Transform plane2;
    [SerializeField] private float spawnTime = 4.0f;
    private float elapsedTime = 0f;


    private void Update()
    {
        plane1.Translate(new Vector3(0f, 0f, -10f) * Time.deltaTime);
        plane2.Translate(new Vector3(0f, 0f, -10f) * Time.deltaTime);

        if(plane1.transform.position.z <= -200f)
        {
            plane1.transform.position = new Vector3(0f, 0f, 200f);
        }
        if (plane2.transform.position.z <= -200f)
        {
            plane2.transform.position = new Vector3(0f, 0f, 200f);
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime > spawnTime)
        {
            float x = Random.Range(-3f, 3f);
            PoolManager.Instance.SpawnObject<TestCube>(transform, new Vector3(x, 0.5f, 10f));
            elapsedTime = 0f;
        }
    }
}
    