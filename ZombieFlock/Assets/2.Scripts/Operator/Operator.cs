using TKCamera;
using UnityEngine;
using System.Collections.Generic;
using System;


public class Operator : MonoSingleton<Operator>
{
    [SerializeField] private bool isDevMode;
    private GameState gameState;
    public static event Action OnPostInGame;
    public static event Action OnPreTitle;
    
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
        //TODO : 이거 Json으로 로드되게 전환
        //=======================Quest=========================================
        //Chapter0 : Tutorial
        //todo : 이거 타겟 위치들 다 데이터 화 하고 스폰하도록 하자.
        QuestConditionGetItem chapter0_condition1 = new QuestConditionGetItem(5, 1, new Vector3(-1.49f, 4.776f, 3.82f)); //AK-47 = 10
        QuestConditionGetItem chapter0_condition2 = new QuestConditionGetItem(10, 1, new Vector3(-5.024f, 4.864f, 11.2588f)); //Shotgun = 10
        QuestConditionActiveEvent chapter0_condition3 = new QuestConditionActiveEvent(QuestEventType.Tutorial_Equipped);
        QuestConditionActiveEvent chapter0_condition4 = new QuestConditionActiveEvent(QuestEventType.Tutorial_Aim);
        QuestConditionActiveEvent chapter0_condition5 = new QuestConditionActiveEvent(QuestEventType.Tutorial_Shot);
        QuestConditionKill chapter0_condition6 = new QuestConditionKill(1, 1); //NormalZombie = 1
        QuestConditionActiveEvent chapter0_condition7 = new QuestConditionActiveEvent(QuestEventType.EnterEndPoint, new Vector3(24.87f, 12.96f, 41.17f));

        List<QuestCondition> chapter0_conditionList1 = new List<QuestCondition>();
        chapter0_conditionList1.Add(chapter0_condition1);
        List<QuestCondition> chapter0_conditionList2 = new List<QuestCondition>();
        chapter0_conditionList2.Add(chapter0_condition2);
        List<QuestCondition> chapter0_conditionList3 = new List<QuestCondition>();
        chapter0_conditionList3.Add(chapter0_condition3);
        List<QuestCondition> chapter0_conditionList4 = new List<QuestCondition>();
        chapter0_conditionList4.Add(chapter0_condition4);
        List<QuestCondition> chapter0_conditionList5 = new List<QuestCondition>();
        chapter0_conditionList5.Add(chapter0_condition5);
        List<QuestCondition> chapter0_conditionList6 = new List<QuestCondition>();
        chapter0_conditionList6.Add(chapter0_condition6);
        List<QuestCondition> chapter0_conditionList7 = new List<QuestCondition>();
        chapter0_conditionList7.Add(chapter0_condition7);
        Quest chapter0_quest1 = new Quest(1, "[Tutorial Quest 1]", "Acquire 1 AK47", chapter0_conditionList1);
        Quest chapter0_quest2 = new Quest(2, "[Tutorial Quest 2]", "Acquire 1 Shotgun", chapter0_conditionList2);
        Quest chapter0_quest3 = new Quest(3, "[Tutorial Quest 3]", "Equipped Gun", chapter0_conditionList3);
        Quest chapter0_quest4 = new Quest(4, "[Tutorial Quest 4]", "On Aim", chapter0_conditionList4);
        Quest chapter0_quest5 = new Quest(5, "[Tutorial Quest 5]", "On Shot", chapter0_conditionList5);
        Quest chapter0_quest6 = new Quest(6, "[Tutorial Quest 6]", "Kill a single normal zombie", chapter0_conditionList6);
        Quest chapter0_quest7 = new Quest(7, "[Tutorial Quest 7]", "Enter End Point", chapter0_conditionList7);
        List<Quest> chapter0_questList = new List<Quest>();
        chapter0_questList.Add(chapter0_quest1);
        chapter0_questList.Add(chapter0_quest2);
        chapter0_questList.Add(chapter0_quest3);
        chapter0_questList.Add(chapter0_quest4);
        chapter0_questList.Add(chapter0_quest5);
        chapter0_questList.Add(chapter0_quest6);
        chapter0_questList.Add(chapter0_quest7);


        //Chapter1 : 
        QuestConditionGetItem chapter01_condition1 = new QuestConditionGetItem(10, 1); //Shotgun = 10
        QuestConditionKill chapter01_condtion2 = new QuestConditionKill(90, 1); //BossZombie = 90

        List<QuestCondition> chapter01_conditionList1 = new List<QuestCondition>();
        chapter01_conditionList1.Add(chapter01_condition1);
        List<QuestCondition> chapter01_conditionList2 = new List<QuestCondition>();
        chapter01_conditionList2.Add(chapter01_condtion2);
        Quest quest1 = new Quest(10, "[Chapter 1 Quest 1]", "Acquire 1 Shotgun", chapter01_conditionList1);
        Quest quest2 = new Quest(11, "[Chapter 1 Quest 2]", "Kill Black Bull", chapter01_conditionList2);
        List<Quest> chapter1_questList = new List<Quest>();
        chapter1_questList.Add(quest1);
        chapter1_questList.Add(quest2);
        //=======================Chapter=========================================
        List<Chapter> chapterData = new List<Chapter>();
        Chapter chapter0 = new Chapter(0, "Wake up", "welcome! tutorial!", new Vector3(-3.45f, 4.72f, 1.22f));
        Chapter chapter1 = new Chapter(1, "CodeName : Black Bull",
            "\"Code-named Black Bull.\" To secure the abandoned Nellis Air Force Base, the most important thing is to eliminate the mutant zombie “Black Bull”.", 
            new Vector3(193f, 0f, -95.1f));
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

            questManager.CheckCurrentQuestProgress();
        }

    }

    public void ChangeGameState(GameState state)
    {
        if(gameState == state)
        {
            return;
        }

        gameState = state;
        if(state == GameState.InGame)
        {
            chapterManager.OnLoadChapterLinear();  
        }
        else if(state == GameState.Title)
        {
            //TODO
        }        
    }

    //@tk 다른 방법 강구
    public  void OnUpdateChapter()
    {
        chapterManager.CurrentChapterID = chapterManager.CurrentChapterID + 1;
    }

    public void SetPostLoadScene(Chapter chapter)
    {
        //Player Setting
        Vector3 spawnPos = chapter.playerSpawnPosition;
        Instantiate(cameraObj, spawnPos, Quaternion.identity);
        ingamePlayer = Instantiate(playerObj, spawnPos, Quaternion.identity).GetComponent<PlayerManager>();
        UIManager.HUD.ResetWeaponHUD();
        ingamePlayer.OnUpdateWeapon += UIManager.HUD.OnUpdateWeaponHUD;

        //Monster Trigger Setting
        SpawnTrigger[] spawnTriggers = FindObjectsByType<SpawnTrigger>(FindObjectsSortMode.None);
        if (spawnTriggers != null && spawnTriggers.Length > 0)
        {
            foreach (var trigger in spawnTriggers)
            {
                trigger.InitSpawnTrigger();
            }
        }

        //UI Setting
        OnPostInGame?.Invoke();
    }
}
