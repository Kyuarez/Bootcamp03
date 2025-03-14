using UnityEngine;

public class Operator : MonoSingleton<Operator>
{
    [SerializeField] private bool isDevMode;

    private PlayerManager ingamePlayer;
    private PoolManager poolManager;
    private PatrolPointManager patrolManager;

    public bool IsDevMode
    {
        get
        {
            return isDevMode;
        }
    }

    public PlayerManager PlayerManager
    {
        get 
        {
            //@tk �̰� ���߿� �� ��ȯ �� �� ���� player �޾ƿ��� ��� �ʿ�
            if (ingamePlayer == null) 
            {
                ingamePlayer = Object.FindFirstObjectByType<PlayerManager>();
            }
            
            return ingamePlayer; 
        }
    }

    public PatrolPointManager PatrolManager
    {
        get
        {
            //@tk �̰� ���߿� �� ��ȯ �� �� ���� patrolManager�� �޾ƿ��� ��� �ʿ�
            if (patrolManager == null)
            {
                patrolManager = Object.FindFirstObjectByType<PatrolPointManager>();
            }

            return patrolManager;
        }
    }

    protected override void Awake()
    {
        //Bind
        poolManager = Object.FindAnyObjectByType<PoolManager>();

        //Init
        //poolManager.Init();
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Q) == true)
        //{

        //}
    }
}
