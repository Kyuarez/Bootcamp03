using TMPro;
using UnityEngine;

public class UIHUDWeapon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bulletText;
    [SerializeField] private TextMeshProUGUI magazineText;
    [SerializeField] private TextMeshProUGUI weaponNameText;

    private void Start()
    {
        
    }

    public void SetActiveState(bool active)
    {
        if (gameObject.activeSelf == !active)
        {
            gameObject.SetActive(active);
        }
    }

    public void ResetWeaponHUD()
    {
        bulletText.text = string.Empty;
        magazineText.text = string.Empty;
        weaponNameText.text = string.Empty;
    }

    //@tk : 나중엔 업데이트 문에서 매 프레임마다 체크가 아니라, 옵저버 패턴 등으로...
    public void UpdateWeaponHUD()
    {
        if (Operator.Instance.PlayerManager.CurrentWeapon == null)
        {
            ResetWeaponHUD();
            return;
        }

        Gun gun = Operator.Instance.PlayerManager.CurrentWeapon;
        bulletText.text = $"{gun.CurrentBulletCount}/{gun.MaxBulletCount}";
        magazineText.text = $"{gun.CurrentMagazineCount}/{gun.MaxMagazineCount}";
        weaponNameText.text = $"Current : {gun.GunName}";
    }
}
