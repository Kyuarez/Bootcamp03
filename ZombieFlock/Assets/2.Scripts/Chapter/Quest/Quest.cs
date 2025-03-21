using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 퀘스트 정보 데이터 클래스
/// </summary>
[Serializable]
public class Quest 
{
    public int QuestID;
    public string QuestTitle;
    public string QuestDescription;
    //퀘스트 조건 자료구조 가지게 하기
    public List<QuestCondition> Conditions;

    private bool isCompleted;
    public bool IsCompleted
    {
        get
        {
            foreach (var condition in Conditions)
            {
                if(condition.CheckCondition() == false)
                {
                    return false;
                }
            }

            isCompleted = true;
            return true;
        }
    }



    public Quest(int questID, string questTitle, string questDescription)
    {
        QuestID = questID;
        QuestTitle = questTitle;
        QuestDescription = questDescription;
        isCompleted = false;
        //TODO : QuestID 로 QuestCondition Load
        Conditions = new List<QuestCondition>();
    }

    public Quest(int questID, string questTitle, string questDescription, List<QuestCondition> Conditions)
    {
        QuestID = questID;
        QuestTitle = questTitle;
        QuestDescription = questDescription;
        isCompleted = false;
        //TODO : QuestID 로 QuestCondition Load
        this.Conditions = Conditions;
    }

    
}
