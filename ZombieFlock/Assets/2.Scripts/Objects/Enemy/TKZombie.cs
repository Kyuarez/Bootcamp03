using UnityEngine;

public class TKZombie : MonoBehaviour, IPoolable
{
    [SerializeField] private int hp;
    [SerializeField] private bool isDead;

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
        InitData();
    }

    private void OnDisable()
    {
        ResetData();
    }

    private void InitData()
    {
        //TODO : Init�� Json�̴� ���� ������ �ֱ�.
        hp = 100;
        isDead = false;
    }

    private void ResetData() 
    {
        hp = -1;
        isDead = true;
    }

    //@tk : TODO ���� ����ϰ� �ڵ� © �ʿ�...
    public void Damage(int damage) 
    {
        hp -= damage;
        Debug.LogFormat($"{GetType().Name}'s hp : {hp}");
    }


    public void Dead()
    {
        PoolManager.Instance.DeSpawnObject(this);
    }

    private void Update()
    {
        if(isDead == true)
        {
            return;
        }

        if(hp <= 0)
        {
            isDead = true;
            PoolManager.Instance.DeSpawnObject(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            Debug.Log("OnCollision");
        }
    }
}
