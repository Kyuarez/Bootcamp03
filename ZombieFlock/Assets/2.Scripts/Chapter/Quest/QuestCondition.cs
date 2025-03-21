using UnityEngine;

public abstract class QuestCondition 
{
    public QuestConditionType ConditionType { get; protected set; }
    public abstract bool CheckCondition();
    public abstract string GetDescription();
}

public class QuestConditionGetItem : QuestCondition
{
    public int ItemID {  get; set; }
    public int RequiredAmount {  get; set; }
    public int CurrentAmount {  get; set; }

    public QuestConditionGetItem(int ItemID, int RequiredAmount)
    {
        this.ItemID = ItemID;
        this.RequiredAmount = RequiredAmount;
        this.CurrentAmount = 0;
        ConditionType = QuestConditionType.GetItem;
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

    public QuestConditionKill(int TargetId, int RequiredKills)
    {
        this.TargetId = TargetId;
        this.RequiredKills = RequiredKills;
        this.CurrentKills = 0;
        ConditionType = QuestConditionType.Kill;

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

    public QuestConditionActiveEvent(string eventName)
    {
        this.EventName = eventName;
        this.IsActive = false;
        ConditionType = QuestConditionType.ActiveEvent;
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
