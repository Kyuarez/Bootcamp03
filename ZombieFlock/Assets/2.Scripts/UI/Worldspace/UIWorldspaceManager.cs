using UnityEngine;

public class UIWorldspaceManager : MonoSingleton<UIWorldspaceManager>
{
    private Canvas canvas;

    private UIQuestMarker questMarker;
    private UIInteraction interaction;

    protected override void Awake()
    {
        base.Awake();

        canvas = GetComponent<Canvas>();
        questMarker = GetComponentInChildren<UIQuestMarker>();
        interaction = GetComponentInChildren<UIInteraction>();


        interaction.ResetInteractionUI();
    }

    private void Start()
    {
        Operator.OnPostInGame += questMarker.OnPostInGame;
        Operator.OnPostInGame += interaction.OnPostInGame;

        Operator.Instance.QuestManager.OnChangeQuest += questMarker.OnChangeQuest;
    }

    public void ResetInteractionUI()
    {
        interaction.ResetInteractionUI();
    }

    public void OnInteractionUI(KeyCode keyCode, Vector3 targetPos = default(Vector3)) 
    {
        interaction.OnInteractionUI(keyCode, targetPos);
    }
}
