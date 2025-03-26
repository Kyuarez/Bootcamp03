using UnityEngine;

public abstract class QuestCondition 
{
    public QuestConditionType ConditionType { get; protected set; }
    public Vector3 TargetPosition { get; protected set; }
    public abstract bool CheckCondition();
    public abstract string GetDescription();
}

public class QuestConditionGetItem : QuestCondition
{
    public int ItemID {  get; set; }
    public int RequiredAmount {  get; set; }
    public int CurrentAmount {  get; set; }

    public QuestConditionGetItem(int ItemID, int RequiredAmount, Vector3 targetPosition = default(Vector3))
    {
        this.ConditionType = QuestConditionType.GetItem;
        this.ItemID = ItemID;
        this.RequiredAmount = RequiredAmount;
        this.CurrentAmount = 0;
        this.TargetPosition = targetPosition;
    }

    public override bool CheckCondition()
    {
        return CurrentAmount >= RequiredAmount;        
    }

    public override string GetDescription()
    {
        return $"Condition : Get Item ({CurrentAmount} / {RequiredAmount})";
    }

    public void UpdateCurrentAmount(int amount)
    {
        CurrentAmount = Mathf.Clamp(CurrentAmount + amount, 0, RequiredAmount);
        Debug.Log(GetDescription());
    } 
}

public class QuestConditionKill : QuestCondition
{
    public int TargetId { get; set; }
    public int RequiredKills { get; set; }
    public int CurrentKills { get; set; }

    public QuestConditionKill(int TargetId, int RequiredKills, Vector3 targetPosition = default(Vector3))
    {
        this.ConditionType = QuestConditionType.Kill;
        this.TargetId = TargetId;
        this.RequiredKills = RequiredKills;
        this.CurrentKills = 0;
        this.TargetPosition = targetPosition;
    }

    public override bool CheckCondition()
    {
        return CurrentKills >= RequiredKills;
    }

    public override string GetDescription()
    {
        return $"Condition : Kill ({CurrentKills} / {RequiredKills})";
    }

    public void UpdateCurrentAmount(int amount)
    {
        CurrentKills = Mathf.Clamp(CurrentKills + amount, 0, RequiredKills);
        Debug.Log(GetDescription());
    }
}

public class QuestConditionActiveEvent : QuestCondition
{
    public string EventName { get; set; }
    public bool IsActive { get; set; }

    public QuestConditionActiveEvent(string eventName, Vector3 targetPosition = default(Vector3))
    {
        ConditionType = QuestConditionType.ActiveEvent;
        this.EventName = eventName;
        this.IsActive = false;
        this.TargetPosition = targetPosition;
    }

    public override bool CheckCondition()
    {
        return IsActive;
    }

    public override string GetDescription()
    {
        return $"Condition : Active Event (active? : {IsActive})";
    }
}
