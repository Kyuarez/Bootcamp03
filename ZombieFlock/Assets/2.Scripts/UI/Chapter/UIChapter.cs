using System.Collections;
using TMPro;
using UnityEngine;

public class UIChapter : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI chapterTitle;

    [Header("factor")]
    [SerializeField] private float waitTime = 1.0f;
    [SerializeField] private float fadeTime = 2.0f;
    [SerializeField] private float duration = 2.0f;

    private Coroutine coroutine;


    private void Awake()
    {
        ResetUIChapter();
    }

    public void PreSetUIChpater(int chapterID, string title)
    {
        chapterTitle.text = $"Chapter {chapterID} : {title}";
    }

    public void OnUIChapter()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

         coroutine = StartCoroutine(OnUIChapterCo());
    }

    private void ResetUIChapter()
    {
        chapterTitle.text = string.Empty;
        chapterTitle.color = new Color(1, 1, 1, 0);

        if (panel.activeSelf == true)
        {
            panel.SetActive(false);
        }

        coroutine = null;
    }

    private IEnumerator OnUIChapterCo()
    {
        yield return new WaitForSeconds(waitTime);
        panel.SetActive(true);
        float elasedTime = 0f;
        
        while(elasedTime < fadeTime)
        {
            elasedTime += Time.deltaTime;
            float a = Mathf.SmoothStep(0, 1, (elasedTime / fadeTime));
            chapterTitle.color = new Color(1, 1, 1, a);
            yield return null;
        }
        chapterTitle.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(duration);
        
        elasedTime = 0f;
        while (elasedTime < fadeTime)
        {
            elasedTime += Time.deltaTime;
            float a = Mathf.SmoothStep(1, 0, (elasedTime / fadeTime));
            chapterTitle.color = new Color(1, 1, 1, a);
            yield return null;
        }

        ResetUIChapter();
    }
}
