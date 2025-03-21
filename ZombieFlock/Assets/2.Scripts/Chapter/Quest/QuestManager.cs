using System.Collections.Generic;
using UnityEngine;

/*
 챕터 매니저가 Quest 매니저에게 Quest Bundle을 제공하면, QuestManager에서 차례대로 업데이트
 */
public class QuestManager
{
    public Quest currentQuest;
    public int currentIndex;
    public List<Quest> QuestData = new List<Quest>();

    public bool IsCompletedCurrentQuest
    {
        get
        {
            foreach (Quest quest in QuestData) 
            {
                if(quest.IsCompleted == false)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public void LoadQuestData(Chapter chapter)
    {
        QuestData.Clear();
        currentQuest = null;
        currentIndex = 0;

        QuestData = chapter.QuestBundle;

        if (QuestData == null || QuestData.Count <= 0)
        {
            Debug.Assert(false, $"{chapter.ChapterID}'s QuestBundle is empty");
            return;
        }

        currentIndex = 0;
        currentQuest = QuestData[currentIndex];
    }

    public void UpdateCurrentQuestGetItem(int itemID, int amount = 1)
    {
        foreach (var questCondition in currentQuest.Conditions)
        {
            if (questCondition.ConditionType == QuestConditionType.GetItem)
            {
                QuestConditionGetItem condition = (QuestConditionGetItem)questCondition;
                if (condition.ItemID == itemID)
                {
                    condition.UpdateCurrentAmount(amount);
                }
            }
        }
    }
    /// <summary>
    /// TargetID = ObjectID
    /// </summary>
    public void UpdateCurrentQuestKill(int targetID, int amount = 1)
    {
        foreach (var questCondition in currentQuest.Conditions)
        {
            if (questCondition.ConditionType == QuestConditionType.Kill)
            {
                QuestConditionKill condition = (QuestConditionKill)questCondition;
                if (condition.TargetId == targetID)
                {
                    condition.UpdateCurrentAmount(amount);
                }
            }
        }
    }
    public void UpdateCurrentQuestActiveEvent(string eventName)
    {
        foreach (var questCondition in currentQuest.Conditions)
        {
            if (questCondition.ConditionType == QuestConditionType.ActiveEvent)
            {
                QuestConditionActiveEvent condition = (QuestConditionActiveEvent)questCondition;
                condition.IsActive = true;
            }
        }
    }

    public void SetNextQuest()
    {
        currentIndex++;

        if (currentIndex == QuestData.Count)
        {
            //TODO : Chapter Clear
            return;
        }

        if (QuestData[currentIndex] == null)
        {
            Debug.Assert(false, $"{QuestData}'s {currentIndex} is null");
            return;
        }

        currentQuest = QuestData[currentIndex];
    }
}
