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

    public List<Quest> QuestBundle;
}
