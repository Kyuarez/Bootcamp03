using System.Collections.Generic;
using UnityEngine;

//@tk Operator가 가지고 있게 하자.
public class ChapterManager 
{
    public void InitChapterData()
    {
        //Load하는 법
    }

    public Chapter GetChapter(int chapterID)
    {
        if(chapterDict.ContainsKey(chapterID) == false)
        {
            return null;
        }

        return chapterDict[chapterID];
    }



    /// <summary>
    /// key : Chapter ID, value : Chapter
    /// </summary>
    private Dictionary<int, Chapter> chapterDict = new Dictionary<int, Chapter>();

}
