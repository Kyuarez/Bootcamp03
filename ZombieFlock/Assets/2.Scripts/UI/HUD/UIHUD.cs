using TMPro;
using UnityEngine;

/* HUD
 ���� �г� : ���� �÷��̾ ���� ���� (Health ��)
 ���� �г� : ���� ���⿡ ���� ����  
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


    //@tk : ���߿� ������Ʈ ������ �� �����Ӹ��� üũ�� �ƴ϶�, ������ ���� ������...
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
