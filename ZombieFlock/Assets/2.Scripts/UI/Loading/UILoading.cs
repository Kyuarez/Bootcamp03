using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image coverImage;

    private float fadeDuration = 0.5f;
    private bool isFading = false;

    public void ResetUILoading() 
    {
        if(panel.activeSelf == true)
        {
            panel.SetActive(false);
        }

        coverImage.color = new Color(0, 0, 0, 0);
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
