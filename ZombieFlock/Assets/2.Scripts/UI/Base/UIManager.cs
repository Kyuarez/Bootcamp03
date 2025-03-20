using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public static UIIngameMenu InGameMenu;
    public static UICrossHair CrossHair;

    protected override void Awake()
    {
        base.Awake();

        InGameMenu = GetComponentInChildren<UIIngameMenu>();
        CrossHair = GetComponentInChildren<UICrossHair>();
    }
}
