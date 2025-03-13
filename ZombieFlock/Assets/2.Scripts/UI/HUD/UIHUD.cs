using TMPro;
using UnityEngine;

/* HUD
 좌측 패널 : 현재 플레이어에 대한 상태 (Health 등)
 우측 패널 : 현재 무기에 대한 상태  
 */
public class UIHUD : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private TextMeshProUGUI bulletText;
    [SerializeField] private TextMeshProUGUI magazineText;
    [SerializeField] private TextMeshProUGUI weaponNameText;

    private void Awake()
    {
        ResetWeaponHUD();
    }

    private void Update()
    {
        UpdateWeaponHUD();
    }


    //@tk : 나중엔 업데이트 문에서 매 프레임마다 체크가 아니라, 옵저버 패턴 등으로...
    private void UpdateWeaponHUD()
    {
        if(Operator.Instance.PlayerManager.CurrentWeapon == null)
        {
            ResetWeaponHUD();
            return;
        }

        Gun gun = Operator.Instance.PlayerManager.CurrentWeapon;
        bulletText.text = $"{gun.CurrentBulletCount}/{gun.MaxBulletCount}";
        magazineText.text = $"{gun.CurrentMagazineCount}/{gun.MaxMagazineCount}";
        weaponNameText.text = $"Current Weapon : {gun.CurrentGunName}";
    }

    private void ResetWeaponHUD()
    {
        bulletText.text = string.Empty;
        magazineText.text = string.Empty;
        weaponNameText.text = string.Empty;
    }
}
