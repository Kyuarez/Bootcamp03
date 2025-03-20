using TKCamera;
using UnityEngine;

public class Operator : MonoSingleton<Operator>
{
    [SerializeField] private bool isDevMode;

    private PlayerManager ingamePlayer;
    private UIManager uiManager;
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

    public UIManager UIManager
    {
        get { return uiManager; }
    }

    protected override void Awake()
    {
        //TODO : 이거 이제 Awake 할 때랑 Scene에서 받을 것이랑 구분해야 함.
        //Bind
        poolManager = Object.FindAnyObjectByType<PoolManager>();
        uiManager = Object.FindAnyObjectByType<UIManager>();

        cameraShake = Camera.main.GetComponent<CameraShake>();

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            bool isPause = UIManager.InGameMenu.OnIngameMenu();
            Time.timeScale = (isPause == true) ? 0 : 1;
        }
    }
}
