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
    public void UpdateWeaponHUD(Gun gun)
    {
        bulletText.text = $"{gun.CurrentBulletCount}/{gun.MaxBulletCount}";
        magazineText.text = $"{gun.CurrentMagazineCount}/{gun.MaxMagazineCount}";
        weaponNameText.text = $"Current : {gun.GunName}";
    }

    public void SetVisibleHUD(bool visible)
    {
        Color color = (visible == true) ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0f);
        bulletText.color = color;
        magazineText.color = color;
        weaponNameText.color = color;
    }
}
