using UnityEngine;
using UnityEngine.UI;

public class UICrossHair : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image crossHairImage;

    private void Update()
    {
        if(Operator.Instance.PlayerManager.IsAim == true)
        {
            panel.SetActive(true);
        
            //@tk Ray

        }
        else
        {
            panel.SetActive(false);
        }
    }


}
