using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject InGameUI;

    public static UIIngameMenu InGameMenu;
    public static UICrossHair CrossHair;
    public static UIChapter Chapter;
    public static UIHUD HUD;
    public static UIEnding Ending;

    protected override void Awake()
    {
        base.Awake();

        InGameMenu = GetComponentInChildren<UIIngameMenu>();
        CrossHair = GetComponentInChildren<UICrossHair>();
        Chapter = GetComponentInChildren<UIChapter>();
        HUD = GetComponentInChildren<UIHUD>();
        Ending = GetComponentInChildren<UIEnding>();

        Operator.OnPreTitle += OnTitle;
        Operator.OnPostInGame += OnInGame;
        Operator.OnPostInGame += Chapter.OnUIChapter;

        Operator.Instance.QuestManager.OnChangeQuest += HUD.OnChangeQuestHUD;
    }

    public void OnInGame()
    {
        if (InGameUI.activeSelf == false)
        {
            InGameUI.SetActive(true);
        }
    }
    public void OnTitle()
    {
        if (InGameUI.activeSelf == true)
        {
            InGameUI.SetActive(false);
        }
    }
}
