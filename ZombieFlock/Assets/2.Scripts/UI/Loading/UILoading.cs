using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image coverImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI chapterHeaderText;
    [SerializeField] private TextMeshProUGUI chapterDescText;

    [SerializeField] private Camera uiCam;

    private float fadeDuration = 0.5f;
    private bool isFading = false;

    public void ResetUILoading() 
    {
        
        if(backgroundImage.gameObject.activeSelf == true)
        {
            backgroundImage.gameObject.SetActive(false);
        }

        chapterHeaderText.text = string.Empty;
        chapterDescText.text = string.Empty;

        coverImage.color = new Color(0, 0, 0, 0);
    }

    public void OnLoadingPage(Chapter chapter)
    {
        chapterHeaderText.text = chapter.ChapterTitle;
        chapterDescText.text = chapter.ChapterDesc;

        backgroundImage.gameObject.SetActive(true);
        //TODO. On Loading Anim
    }

    public IEnumerator FadeInAndLoadSceneCo(float delay)
    {
        isFading = true;
        yield return StartCoroutine(FadeCover(0, 1, fadeDuration));
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(FadeCover(1, 0, fadeDuration));
        isFading = false;
    }

    public IEnumerator FadeCover(float startAlpha, float endAlpha, float duration)
    {
        if (panel.activeSelf == false)
        {
            panel.SetActive(true);
        }

        float elapsedTime = 0.0f;
        Color coverColor = coverImage.color;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            coverColor.a = a;
            coverImage.color = coverColor;
            yield return null;
        }

        coverColor.a = endAlpha;
        coverImage.color = coverColor;
    }

}
