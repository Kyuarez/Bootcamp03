using TKCamera;
using UnityEngine;
using System.Collections.Generic;

public class Operator : MonoSingleton<Operator>
{
    [SerializeField] private bool isDevMode;

    private PlayerManager ingamePlayer;
    private UIManager uiManager;
    private PoolManager poolManager;
    private PatrolPointManager patrolManager;
    private CameraShake cameraShake;

    private QuestManager questManager;

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

    public QuestManager QuestManager
    {
        get { return questManager; }
    }

    #region Test
    private Chapter testChapter;
    #endregion

    protected override void Awake()
    {
        //TODO : 이거 이제 Awake 할 때랑 Scene에서 받을 것이랑 구분해야 함.
        //Bind
        poolManager = Object.FindAnyObjectByType<PoolManager>();
        uiManager = Object.FindAnyObjectByType<UIManager>();

        cameraShake = Camera.main.GetComponent<CameraShake>();

        #region Test
        questManager = new QuestManager();

        testChapter = new Chapter()
        {
            ChapterID = 1,
            sceneName = "Level0306",
            isClearChapter = false,
        };

        QuestConditionGetItem condition1 = new QuestConditionGetItem(10, 1); //Shotgun = 10
        QuestConditionKill condtion2 = new QuestConditionKill(1, 5); //NormalZombie = 1

        List<QuestCondition> conditionList1 = new List<QuestCondition>();
        conditionList1.Add(condition1);
        List<QuestCondition> conditionList2 = new List<QuestCondition>();
        conditionList2.Add(condtion2);
        Quest quest1 = new Quest(1, "[샷건 먹기]", "샷건 1회 먹기", conditionList1);
        Quest quest2 = new Quest(2, "[좀비 죽이기]", "좀비 10마리 죽이기", conditionList2);
        List<Quest> questList = new List<Quest>();
        questList.Add(quest1);
        questList.Add(quest2);

        testChapter.LoadQuestBundle(questList);

        #endregion
    }

    private void Update()
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
