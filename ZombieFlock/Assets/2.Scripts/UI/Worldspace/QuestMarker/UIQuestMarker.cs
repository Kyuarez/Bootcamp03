using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class UIQuestMarker : MonoBehaviour
{
    private Vector3 targetPosition;
    private Camera mainCam;

    private Image markIcon;

    private void Awake()
    {
        markIcon = GetComponent<Image>();
    }

    public void OnPostInGame()
    {
        mainCam = Camera.main;
    }

    //@tk TODO : 일단 퀘스트 별 단일 조건이라서, 퀘스트로 받음. 나중엔 QuestCondition event 따로 처리
    public void OnChangeQuest(Quest quest)
    {
        if(quest == null)
        {
            targetPosition = default(Vector3);
            return;
        }

        //TODO : change Target
        if(quest.Conditions == null || quest.Conditions.Count <= 0)
        {
            targetPosition = default(Vector3);
            return;
        }

        Vector3 targetPos = quest.Conditions[0].TargetPosition;
        if(targetPos == default(Vector3) || targetPos == null)
        {
            targetPosition = default(Vector3);
            return;
        }

        targetPosition = targetPos;
    }

    private void Update()
    {
        if(mainCam == null || targetPosition == null || targetPosition == default(Vector3))
        {
            if(markIcon.enabled == true)
            {
                markIcon.enabled = false;
            }
            return;
        }

        Vector3 screenPosition = mainCam.WorldToScreenPoint(targetPosition);
        if (screenPosition.z > 0) // z가 0보다 커야 카메라의 앞에 있음
        {
            markIcon.enabled = true;
            transform.position = screenPosition;
        }
        else
        {
            markIcon.enabled = false;
        }

    }
}
