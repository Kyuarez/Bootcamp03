using UnityEngine;

public class UIHelp : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private bool OnHelpUI = false;
    
    private void Awake()
    {
        OnHelpUI = Operator.Instance.IsDevMode;

        panel.SetActive(OnHelpUI);
    }
}
