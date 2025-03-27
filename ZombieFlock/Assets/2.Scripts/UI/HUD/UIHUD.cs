using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;

/* HUD
 좌측 패널 : 현재 플레이어에 대한 상태 (Health 등)
 우측 패널 : 현재 무기에 대한 상태  
 */
public class UIHUD : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private UIHUDState HUDState;
    [SerializeField] private UIHUDWeapon HUDWeapon;
    [SerializeField] private UIHUDQuest HUDQuest;


    private void Awake()
    {
        HUDWeapon.ResetWeaponHUD();
        HUDQuest.ResetQuestHUD();
    }

    public void OnChangeQuestHUD(Quest quest)
    {
        HUDQuest.OnQuestHUD(quest);
    }

    public void ResetWeaponHUD()
    {
        HUDWeapon.ResetWeaponHUD();
    }
    
    public void OnUpdateWeaponHUD(Gun gun)
    {
        HUDWeapon.UpdateWeaponHUD(gun);
    }

    //@TK 기능은 유지하되, 이미지만 잠시 안보이게
    public void SetVisibleHUD(bool onCutscene)
    {
        HUDWeapon.SetVisibleHUD(!onCutscene);
        HUDQuest.SetVisibleHUD(!onCutscene);
    }
    
}
