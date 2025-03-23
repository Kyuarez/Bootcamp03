using TMPro;
using UnityEngine;

public class UIHUDQuest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questTitleText;
    [SerializeField] private TextMeshProUGUI questDescriptionText;

    public void SetActiveState(bool active)
    {
        if (gameObject.activeSelf == !active)
        {
            gameObject.SetActive(active);
        }
    }

    public void ResetQuestHUD()
    {
        questTitleText.text = string.Empty;
        questDescriptionText.text = string.Empty;
        SetActiveState(false);
    }

    public void OnQuestHUD(Quest quest)
    {
        if(quest == null)
        {
            ResetQuestHUD();
            return;
        }

        questTitleText.text = quest.QuestTitle;
        questDescriptionText.text = quest.QuestDescription;
        SetActiveState(true);
    }
}
