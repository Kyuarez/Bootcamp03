using UnityEngine;

public class UIWorldspaceManager : MonoSingleton<UIWorldspaceManager>
{
    private Canvas canvas;

    private UIQuestMarker questMarker;

    protected override void Awake()
    {
        base.Awake();

        canvas = GetComponent<Canvas>();
        questMarker = GetComponentInChildren<UIQuestMarker>();
    }

    private void Start()
    {
        Operator.OnPostInGame += questMarker.OnPostInGame;
        Operator.Instance.QuestManager.OnChangeQuest += questMarker.OnChangeQuest;
    }

}
