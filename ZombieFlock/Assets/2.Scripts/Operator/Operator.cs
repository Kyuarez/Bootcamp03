using TKCamera;
using UnityEngine;

public class Operator : MonoSingleton<Operator>
{
    [SerializeField] private bool isDevMode;

    private PlayerManager ingamePlayer;
    private PoolManager poolManager;
    private PatrolPointManager patrolManager;
    private CameraShake cameraShake;

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
            //@tk 이거 나중엔 씬 전환 할 때 마다 player 받아오는 방식 필요
            if (ingamePlayer == null) 
            {
                ingamePlayer = Object.FindFirstObjectByType<PlayerManager>();
            }
            
            return ingamePlayer; 
        }
    }

    public CameraShake CameraShake
    {
        get
        {
            if(cameraShake == null)
            {
                cameraShake = Camera.main.GetComponent<CameraShake>();
            }
            return cameraShake;
        }
    }

    public PatrolPointManager PatrolManager
    {
        get
        {
            //@tk 이거 나중엔 씬 전환 할 때 마다 patrolManager를 받아오는 방식 필요
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
        cameraShake = Camera.main.GetComponent<CameraShake>();

        
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Q) == true)
        //{

        //}
    }
}
