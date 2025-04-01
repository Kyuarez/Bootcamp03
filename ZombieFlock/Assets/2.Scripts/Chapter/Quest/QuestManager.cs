using System;
using System.Collections.Generic;
using UnityEngine;

/*
 챕터 매니저가 Quest 매니저에게 Quest Bundle을 제공하면, QuestManager에서 차례대로 업데이트
 */
public class QuestManager
{
    private Quest currentQuest;
    public int currentIndex;
    public List<Quest> QuestData = new List<Quest>();

    public event Action<Quest> OnChangeQuest;   //@tk : current Quest Change Action
    //public event Action<QuestCondition> OnChangeQuestCondition;

    public Quest CurrentQuest
    {
        get { return currentQuest; }
        set
        {
            currentQuest = value;
            UpdateQuestEventHandlers(currentQuest);
            OnChangeQuest?.Invoke(currentQuest);
        }
    }

    /// <summary>
    /// 챕터에 있는 모든 퀘스트 클리어
    /// </summary>
    public bool IsCompletedChapterQuest
    {
        get
        {
            if(QuestData == null || QuestData.Count == 0)
            {
                return false;
            }

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
        CurrentQuest = null;
        currentIndex = 0;

        QuestData = chapter.QuestBundle;

        if (QuestData == null || QuestData.Count <= 0)
        {
            Debug.Assert(false, $"{chapter.ChapterID}'s QuestBundle is empty");
            return;
        }

        currentIndex = 0;
        CurrentQuest = QuestData[currentIndex];
    }

    public void UpdateQuestEventHandlers(Quest currentQuest)
    {
        Operator.Instance.PlayerManager.OnGetItem -= UpdateCurrentQuestGetItem;
        ZombieManager.OnDie -= UpdateCurrentQuestKill;
        BossManager.OnDie -= UpdateCurrentQuestKill;
        QuestEventManager.OnEventTriggered -= UpdateCurrentQuestActiveEvent;
        
        
        if (currentQuest == null || currentQuest.Conditions == null || currentQuest.Conditions.Count == 0)
        {
            return;
        }

        foreach (QuestCondition condition in currentQuest.Conditions)
        {
            if(condition.ConditionType == QuestConditionType.GetItem)
            {
                Operator.Instance.PlayerManager.OnGetItem += UpdateCurrentQuestGetItem;
            }
            if (condition.ConditionType == QuestConditionType.Kill)
            {
                ZombieManager.OnDie += UpdateCurrentQuestKill;
                BossManager.OnDie += UpdateCurrentQuestKill;
            }
            if (condition.ConditionType == QuestConditionType.ActiveEvent)
            {
                QuestEventManager.OnEventTriggered += UpdateCurrentQuestActiveEvent;
            }
        }
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
    public void UpdateCurrentQuestActiveEvent(QuestEventType eventType)
    {
        foreach (var questCondition in currentQuest.Conditions)
        {
            if (questCondition.ConditionType == QuestConditionType.ActiveEvent)
            {
                QuestConditionActiveEvent condition = (QuestConditionActiveEvent)questCondition;

                if (condition.EventType == eventType)
                {
                    condition.IsActive = true;
                }
            } 
        }
    }

    public void CheckCurrentQuestProgress()
    {
        if(currentQuest == null)
        {
            return;
        }

        if(currentQuest.IsCompleted == false)
        {
            return;
        }

        SetNextQuest();
    }

    public void SetNextQuest()
    {
        currentIndex = Mathf.Clamp(currentIndex + 1, 0, QuestData.Count);

        if (currentIndex >= QuestData.Count)
        {
            //TODO : Chapter Clear
            Debug.Log("챕터 모든 퀘스트 클리어!");
            CurrentQuest = null;
            Operator.Instance.OnUpdateChapter();
            return;
        }

        if (QuestData[currentIndex] == null)
        {
            Debug.Assert(false, $"{QuestData}'s {currentIndex} is null");
            return;
        }

        CurrentQuest = QuestData[currentIndex];
    }
}   
