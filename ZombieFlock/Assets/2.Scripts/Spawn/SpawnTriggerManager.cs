using System.Collections.Generic;
using UnityEngine;

public class SpawnTriggerManager : MonoSingleton<SpawnTriggerManager>
{
    protected override void Awake()
    {
        base.Awake();
        Operator.OnPostInGame += OnPostInGame;
    }

    public void OnPostInGame()
    {
        triggerDict.Clear();

        SpawnTrigger[] arr = FindObjectsByType<SpawnTrigger>(FindObjectsSortMode.None);
        if(arr == null || arr.Length == 0)
        {
            return;
        }

        foreach (SpawnTrigger trigger in arr)
        {
            triggerDict.Add(trigger.CodeID, trigger);
        }
    }

    public SpawnTrigger GetSpawnTrigger(int codeID)
    {
        if(triggerDict.Count <= 0)
        {
            return null;
        }

        if(triggerDict.ContainsKey(codeID) == false)
        {
            return null;
        }

       return triggerDict[codeID];
    }


    private Dictionary<int, SpawnTrigger> triggerDict = new Dictionary<int, SpawnTrigger>();
}
