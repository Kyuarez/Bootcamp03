using UnityEngine;
using UnityEngine.UI;

public class UICrossHair : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Image crossHairImage;

    [Header("CrossHair")]
    [SerializeField] private Sprite crossHairRifle;
    [SerializeField] private Sprite crossHairShotgun;
    [SerializeField] private Sprite crossHairSniper;

    private void Update()
    {
        if(Operator.Instance.PlayerManager == null)
        {
            return;
        }

        if(Operator.Instance.PlayerManager.IsAim == true)
        {
            panel.SetActive(true);
        
            //TODO : 사정거리 안에 적이 있으면 붉은 색으로 
        }
        else
        {
            panel.SetActive(false);
        }
    }

    public void SetCrossHairByWeapon()
    {
        GunType gunType = Operator.Instance.PlayerManager.CurrentWeapon.CurrentGunType;

        switch (gunType)
        {
            case GunType.Rifle:
                crossHairImage.sprite = crossHairRifle;
                break;
            case GunType.Sniper:
                crossHairImage.sprite = crossHairSniper;
                break;
            case GunType.Shotgun:
                crossHairImage.sprite = crossHairShotgun;
                break;
            default:
                crossHairImage.sprite = crossHairRifle;
                break;
        }
    }

}
