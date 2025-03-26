using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class CutSceneController : MonoBehaviour
{
    [SerializeField] private int codeID;

    private PlayableDirector cutscene;

    private void Awake()
    {
        cutscene = GetComponent<PlayableDirector>();
        cutscene.stopped += ExitCutScene;

        Operator.OnPostInGame += OnPostInGame;
    }

    private void OnDestroy()
    {
        Operator.OnPostInGame -= OnPostInGame;
        cutscene.stopped -= ExitCutScene;
    }

    public void OnPostInGame()
    {
        SpawnTrigger trigger = SpawnTriggerManager.Instance.GetSpawnTrigger(codeID);
        if (trigger == null)
        {
            return;
        }

        trigger.OnPostSpawn += OnPostSpawn;
    }

    public void OnPostSpawn()   
    {
        if(cutscene.playableAsset == null)
        {
            return;
        }

        cutscene.Play();
    }

    public void ExitCutScene(PlayableDirector timeline)
    {
        Destroy(gameObject, 1.0f);
    }
}
