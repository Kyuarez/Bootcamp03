using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// 챕터 데이터 (json이던, SO던 데이터 받아오게 하자.)
/// </summary>
[Serializable]
public class Chapter 
{
    public int ChapterID;
    public string sceneName;
    public bool isClearChapter;

    public List<Quest> QuestBundle = new List<Quest>();

    public void LoadQuestBundle(List<Quest> questBundle)
    {
        this.QuestBundle = questBundle;
        Operator.Instance.QuestManager.LoadQuestData(this);
    }
}
