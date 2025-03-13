using UnityEngine;

public class UICrossHair : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Update()
    {
        if(Operator.Instance.PlayerManager.IsAim == true)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }
}
