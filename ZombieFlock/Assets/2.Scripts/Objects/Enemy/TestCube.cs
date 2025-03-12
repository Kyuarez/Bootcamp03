using UnityEngine;

public class TestCube : MonoBehaviour, IPoolable
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

    private void OnEnable()
    {
        Invoke("DespawnCube", 5f);
    }

    private void Update()
    {
        transform.Translate(new Vector3(0f, 0f, -10f) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //other.gameObject.transform.position = Vector3.zero;
            PlayerManager pManager = other.gameObject.GetComponent<PlayerManager>();

            if(pManager != null)
            {
                pManager.GetComponent<CharacterController>().enabled = false;
                pManager.transform.position = new Vector3(0f, pManager.transform.position.y, 0f);
                pManager.GetComponent<CharacterController>().enabled = true;

                pManager.TestCameraShake();
                pManager.OnAnimEventOneShotSound();
            }

            DespawnCube();
        }
    }

    private void DespawnCube()
    {
        PoolManager.Instance.DeSpawnObject(this);
    }
}
