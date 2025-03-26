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
    public string ChapterTitle;
    public string ChapterDesc;
    public bool isClearChapter;
    public Vector3 playerSpawnPosition; //@tk 플레이어 시작 위치

    public List<Quest> QuestBundle = new List<Quest>();

    public Chapter(int chapterID, string chapterTitle, string chapterDesc, Vector3 playerSpawnPos)
    {
        this.ChapterID = chapterID;
        this.sceneName = "Ingame_" + ChapterID.ToString();
        this.ChapterTitle = chapterTitle;
        this.ChapterDesc = chapterDesc;
        this.playerSpawnPosition = playerSpawnPos;
        this.isClearChapter = false;
    }

    public void LoadQuestBundle(List<Quest> questBundle)
    {
        this.QuestBundle = questBundle;
    }
}
