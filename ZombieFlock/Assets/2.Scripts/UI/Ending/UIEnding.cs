using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEnding : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform creditPanel;
    [SerializeField] private GameObject skipPanel;

    private float originY = -1080f;
    private float maxY = 2000f;
    private float fadeDuration = 2.0f;
    private float creaditDuration = 30.0f;

    private Coroutine endingCoroutine;

    public void ResetEndingCredit()
    {
        creditPanel.anchoredPosition = new Vector2(creditPanel.anchoredPosition.x, originY);
        
        if(panel.activeSelf == true)
        {
            panel.SetActive(false);
        }

        if (skipPanel.activeSelf == true)
        {
            skipPanel.SetActive(false);
        }
    }

    public void OnEndingCredit()
    {
        Cursor.lockState = CursorLockMode.None;

        if(panel.activeSelf == false)
        {
            panel.SetActive(true);
        }

        if (endingCoroutine != null) 
        {
            StopCoroutine(endingCoroutine);
            endingCoroutine = null;
        }

        endingCoroutine = StartCoroutine(OnEndingCreditCo());
    }

    private  IEnumerator OnEndingCreditCo()
    {
        yield return new WaitForSeconds(1.0f);
        float elapsedTime = 0f;

        while (elapsedTime < creaditDuration)
        {
            if(elapsedTime >= (creaditDuration / 4)) //@tk : ending 1/4 지점에 skip 띄우기
            {
                if(skipPanel.activeSelf == false)
                {
                    skipPanel.SetActive(true);
                }
            }

            float newY = Mathf.Lerp(originY, maxY, elapsedTime / creaditDuration);
            creditPanel.anchoredPosition = new Vector2(creditPanel.anchoredPosition.x, newY);

            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        creditPanel.anchoredPosition = new Vector2(creditPanel.anchoredPosition.x, maxY);
        yield return new WaitForSeconds(0.5f);
        SceneTransitionManager.Instance.LoadSceneTitleAsync();
        ResetEndingCredit();
    }

    public void OnClickSkip()
    {
        if (endingCoroutine != null)
        {
            StopCoroutine(endingCoroutine);
            endingCoroutine = null;
        }

        SceneTransitionManager.Instance.LoadSceneTitleAsync();
        ResetEndingCredit();
    }
}
