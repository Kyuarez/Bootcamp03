using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject InGameUI;

    public static UIIngameMenu InGameMenu;
    public static UICrossHair CrossHair;
    public static UIChapter Chapter;

    protected override void Awake()
    {
        base.Awake();

        InGameMenu = GetComponentInChildren<UIIngameMenu>();
        CrossHair = GetComponentInChildren<UICrossHair>();
        Chapter = GetComponentInChildren<UIChapter>();

        Operator.Instance.OnPreTitle += OnTitle;
        
        Operator.Instance.OnPostInGame += OnInGame;
        Operator.Instance.OnPostInGame += Chapter.OnUIChapter;
    }

    public void OnInGame()
    {
        if(InGameUI.activeSelf == false)
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
