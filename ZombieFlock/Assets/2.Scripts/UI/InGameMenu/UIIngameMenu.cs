using UnityEngine;

public class UIIngameMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private bool isOn = false;

    private void Awake()
    {
        if(panel.activeSelf == true)
        {
            panel.SetActive(false);
        }    
        isOn = false;
        
    }

    /// <summary>
    /// return : isOn (true일 떄 pause 해야함)
    /// </summary>
    public bool OnIngameMenu()
    {
        panel.SetActive(!isOn);
        isOn = !isOn;
        return isOn;
    }

    #region OnClick
    public void OnClickExit()
    {
        //TODO : 지금은 그냥 종료인데, 우리는 TitleScene에서 종료하게 할 것임.
        Application.Quit();
    }

    #endregion
}
