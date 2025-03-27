using TMPro;
using UnityEngine;

public class UIHUDQuest : MonoBehaviour
{
    [SerializeField] private GameObject icon;
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

    public void SetVisibleHUD(bool visible)
    {
        icon.SetActive(visible);

        Color color = (visible == true) ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0f);
        questTitleText.color = color;
        questDescriptionText.color = color;
    }
}
