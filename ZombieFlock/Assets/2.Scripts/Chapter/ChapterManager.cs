using System.Collections.Generic;
using UnityEngine;

//@tk Operator가 가지고 있게 하자.
public class ChapterManager
{
    private int currentChapterID;
    private Chapter currentChapter;

    public int CurrentChapterID
    {
        get { return currentChapterID; }
        set 
        {
            if(value >= chapterDict.Count)
            {
                Debug.Log("모든 챕터 클리어");
                UIManager.Ending.OnEndingCredit();
                return;
            }

            currentChapter = GetChapter(value);
            currentChapterID = value;
            OnLoadChapterLinear();
        }
    }

    public ChapterManager()
    {
        currentChapterID = 0;
        currentChapter = GetChapter(currentChapterID);
        LoadChapterData();
    }

    #region Test
    //@tk 25.03.22 : 일단 데이터 매니저 구축 전에는 생성자로 데이터 때려넣기
    public ChapterManager(List<Chapter> chapterData)
    {
        
        if(chapterData == null || chapterData.Count == 0)
        {
            Debug.Assert(false, $"chapter Data doesn't Load");
            return;
        }

        foreach (var chapter in chapterData)
        {
            chapterDict.Add(chapter.ChapterID, chapter);
        }

        currentChapterID = 0;
        currentChapter = GetChapter(currentChapterID);
    }
    #endregion

    public void LoadChapterData()
    {
        //TODO
        //Load하는 법(원래는 Json Data)
    }

    public void OnLoadChapterLinear()   
    {
        SceneTransitionManager.Instance.LoadSceneAsync(currentChapter, OnPostSettingChapter);
    }

    //@tk scene Load 후에 세팅할 것들
    public void OnPostSettingChapter()
    {
        //@tk scene setting -> quest setting -> ui setting
        Operator.Instance.SetPostLoadScene(currentChapter);
        Operator.Instance.QuestManager.LoadQuestData(currentChapter);
        UIManager.Chapter.PreSetUIChpater(currentChapterID, currentChapter.ChapterTitle);
    }
    
    public Chapter GetChapter(int chapterID)
    {
        if(chapterDict.ContainsKey(chapterID) == false)
        {
            Debug.Assert(false, $"chapter ID {currentChapterID} isn't exist");
            return null;
        }

        return chapterDict[chapterID];
    }



    /// <summary>
    /// key : Chapter ID, value : Chapter
    /// </summary>
    private Dictionary<int, Chapter> chapterDict = new Dictionary<int, Chapter>();

}
