using TKCamera;
using UnityEngine;
using System.Collections.Generic;
using System;
using Mono.Cecil;

public class Operator : MonoSingleton<Operator>
{
    [SerializeField] private bool isDevMode;
    private GameState gameState;
    public event Action OnPostInGame;
    public event Action OnPreTitle;

    private UIManager uiManager;
    private PoolManager poolManager;
    private PatrolPointManager patrolManager;
    private CameraShake cameraShake;

    private ChapterManager chapterManager;
    private QuestManager questManager;

    private GameObject playerObj;
    private GameObject cameraObj;
    private PlayerManager ingamePlayer;

    public bool IsDevMode { get { return isDevMode; } }

    public GameState GameState { get { return gameState; } }

    public PlayerManager PlayerManager
    {
        get 
        {
            //@tk 이거 나중엔 씬 전환 할 때 마다 player 받아오는 방식 필요
            if (ingamePlayer == null) 
            {
                ingamePlayer = UnityEngine.Object.FindFirstObjectByType<PlayerManager>();
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
                patrolManager = UnityEngine.Object.FindFirstObjectByType<PatrolPointManager>();
            }

            return patrolManager;
        }
    }

    public UIManager UIManager
    {
        get { return uiManager; }
    }

    public QuestManager QuestManager
    {
        get { return questManager; }
    }

    protected override void Awake()
    {
        base.Awake();

        //TODO : 이거 이제 Awake 할 때랑 Scene에서 받을 것이랑 구분해야 함.
        //Bind
        poolManager = UnityEngine.Object.FindAnyObjectByType<PoolManager>();
        uiManager = UnityEngine.Object.FindAnyObjectByType<UIManager>();

        questManager = new QuestManager();

        playerObj = Resources.Load<GameObject>("Prefabs/Objects/Player");
        cameraObj = Resources.Load<GameObject>("Prefabs/Camera/CameraManager");
        
        #region Test
        //=======================Quest=========================================
        QuestConditionGetItem chapter0_condition1 = new QuestConditionGetItem(10, 1); //Shotgun = 10
        QuestConditionKill chapter0_condtion2 = new QuestConditionKill(1, 5); //NormalZombie = 1

        List<QuestCondition> chapter0_conditionList1 = new List<QuestCondition>();
        chapter0_conditionList1.Add(chapter0_condition1);
        List<QuestCondition> chapter0_conditionList2 = new List<QuestCondition>();
        chapter0_conditionList2.Add(chapter0_condtion2);
        Quest chapter0_quest1 = new Quest(1, "[샷건 먹기]", "샷건 1회 먹기", chapter0_conditionList1);
        Quest chapter0_quest2 = new Quest(2, "[좀비 죽이기]", "좀비 10마리 죽이기", chapter0_conditionList2);
        List<Quest> chapter0_questList = new List<Quest>();
        chapter0_questList.Add(chapter0_quest1);
        chapter0_questList.Add(chapter0_quest2);

        QuestConditionGetItem chapter01_condition1 = new QuestConditionGetItem(10, 1); //Shotgun = 10
        QuestConditionKill chapter01_condtion2 = new QuestConditionKill(1, 5); //NormalZombie = 1

        List<QuestCondition> chapter01_conditionList1 = new List<QuestCondition>();
        chapter01_conditionList1.Add(chapter01_condition1);
        List<QuestCondition> chapter01_conditionList2 = new List<QuestCondition>();
        chapter01_conditionList2.Add(chapter01_condtion2);
        Quest quest1 = new Quest(1, "[샷건 먹기]", "샷건 1회 먹기", chapter01_conditionList1);
        Quest quest2 = new Quest(2, "[좀비 죽이기]", "좀비 10마리 죽이기", chapter01_conditionList2);
        List<Quest> chapter1_questList = new List<Quest>();
        chapter1_questList.Add(quest1);
        chapter1_questList.Add(quest2);
        //=======================Chapter=========================================
        List<Chapter> chapterData = new List<Chapter>();
        Chapter chapter0 = new Chapter(0, "Wake up" , new Vector3(-3.45f, 4.72f, 1.22f));
        Chapter chapter1 = new Chapter(1, "First Mission", new Vector3(193f, 0f, -95.1f));
        chapter0.LoadQuestBundle(chapter0_questList);
        chapter1.LoadQuestBundle(chapter1_questList);
        chapterData.Add(chapter0);
        chapterData.Add(chapter1);
        chapterManager = new ChapterManager(chapterData);
        #endregion

    }

    private void Start()
    {
        gameState = GameState.Title;
        OnPreTitle?.Invoke();
    }

    private void Update()
    {
        if(gameState == GameState.Title)
        {

        }
        else if(gameState == GameState.InGame)
        {
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
                bool isPause = UIManager.InGameMenu.OnIngameMenu();
                Time.timeScale = (isPause == true) ? 0 : 1;
            }

            //TODO : QuestCheck(나중엔 챕터 매니저에서 구현)
            if(questManager.IsCompletedChapterQuest == true)
            {
                Debug.Log("챕터 클리어");
            }

            questManager.CheckCurrentQuestProgress();
        }

    }

    public void ChangeGameState(GameState state)
    {
        gameState = state;
        chapterManager.OnLoadChapterLinear();
    }

    public void SetPostLoadScene()
    {
        Vector3 spawnPos = chapterManager.CurrentChapter.playerSpawnPosition;
        Instantiate(cameraObj, spawnPos, Quaternion.identity);
        ingamePlayer = Instantiate(playerObj, spawnPos, Quaternion.identity).GetComponent<PlayerManager>();
        
        OnPostInGame?.Invoke();
    }
}
