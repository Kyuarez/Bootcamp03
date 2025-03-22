using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject InGameUI;

    public static UIIngameMenu InGameMenu;
    public static UICrossHair CrossHair;

    protected override void Awake()
    {
        base.Awake();

        InGameMenu = GetComponentInChildren<UIIngameMenu>();
        CrossHair = GetComponentInChildren<UICrossHair>();

        Operator.Instance.OnPreTitle += OnTitle;
        Operator.Instance.OnPostInGame += OnInGame;
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
