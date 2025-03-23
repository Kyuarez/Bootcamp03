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

    public void OnUpdateWeaponHUD(Gun gun)
    {
        HUDWeapon.UpdateWeaponHUD(gun);
    }
    
}
