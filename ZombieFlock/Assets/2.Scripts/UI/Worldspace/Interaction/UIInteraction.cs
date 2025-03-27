using TMPro;
using UnityEngine;

public class UIInteraction : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private TextMeshProUGUI interactionText;

    private Vector3 targetPosition;
    private Camera mainCam;

    private float offsetY = 0.5f;

    public void OnPostInGame()
    {
        mainCam = Camera.main;
    }

    public void ResetInteractionUI()
    {
        if(panel.activeSelf == false)
        {
            return;
        }

        targetPosition = Vector3.zero;

        keyText.text = string.Empty;
        interactionText.text = string.Empty;
        
        if(panel.activeSelf == true)
        {
            panel.SetActive(false);
        }
    }

    public void OnInteractionUI(KeyCode key, Vector3 targetPos = default(Vector3))
    {
        //Update TargetPos
        if (targetPos == null || targetPos == default(Vector3))
        {
            //TODO : 플레이어 위치에 두기
            targetPosition = Vector3.zero;
        }
        else
        {
            targetPosition = targetPos;
        }

        //Update Key
        string keyStr = string.Empty;
        string interactionStr = string.Empty;

        switch (key)
        {
            case KeyCode.E:
                keyStr = "E";
                interactionStr = "Pick up";
                break;
            default:
                break;
        }

        keyText.text = keyStr;
        interactionText.text = interactionStr;

        if (panel.activeSelf == false)
        {
            panel.SetActive(true);
        }
    }

    private void Update()
    {
        if (mainCam == null || targetPosition == null)
        {
            if (panel.activeSelf == true)
            {
                panel.SetActive(false);
            }
            return;
        }

        Vector3 screenPosition = mainCam.WorldToScreenPoint(new Vector3(targetPosition.x, targetPosition.y + offsetY, targetPosition.z));
        if (screenPosition.z > 0) // z가 0보다 커야 카메라의 앞에 있음
        {
            transform.position = screenPosition;
        }
    }
}
