using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/* [03.20] @tk 타이틀 로직 : reference -> 울펜슈타인:더 뉴오더 
 타이틀 -> 메인메뉴
 */
public class UIMainMenu : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer titleVideo;

    [Header("Title")]
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private Image titleCoverImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI titleInfoText;
    [SerializeField] private Button titleTouchArea;

    [Header("MainMenu")]
    [SerializeField] private GameObject mainmenuPanel;

    private Coroutine OnTitleCoroutine;
    
    //@tk 나중에 SoundManager에서 처리
    private AudioSource audioSource;

    private float coverFadeTime = 1.0f;
    private float titleFadeTime = 3.0f;
    private float titleInfoFadeTime = 1.0f;
    private float titleWaitTime = 1.0f;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        titleTouchArea.onClick.AddListener(OnClickTitleToMainMenu);

        ResetUITitle();
    }

    private void Start()
    {
        if(OnTitleCoroutine != null)
        {
            StopCoroutine(OnTitleCoroutine);
            OnTitleCoroutine = null;
        }

        OnTitleCoroutine = StartCoroutine(OnTitleCo());
    }

    private void OnDisable()
    {
        ResetUITitle();
    }

    private void ResetUITitle()
    {
        if (titlePanel.activeSelf == false)
        {
            mainmenuPanel.SetActive(true);
        }
        if (mainmenuPanel.activeSelf == true)
        {
            mainmenuPanel.SetActive(false);
        }

        titleCoverImage.color = new Color(0, 0, 0, 1);
        titleText.color = new Color(1, 1, 1, 0);
        titleInfoText.color = new Color(1, 1, 1, 0);

        titleTouchArea.interactable = false;
    }

    private IEnumerator OnTitleCo()
    {
        //Title
        yield return StartCoroutine(PreparedVideoCo());
        yield return StartCoroutine(FadeOutCover());
        yield return StartCoroutine(FadeInTitle(titleText, titleFadeTime));
        yield return new WaitForSeconds(titleWaitTime);
        yield return StartCoroutine(FadeInTitle(titleInfoText, titleInfoFadeTime));
        titleTouchArea.interactable = true;
    }

    private IEnumerator PreparedVideoCo()
    {
        titleVideo.Prepare();

        while(titleVideo.isPrepared == false)
        {
            yield return null;
        }

        titleVideo.Play();
    }

    private IEnumerator FadeOutCover()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < coverFadeTime)
        {
            elapsedTime += Time.deltaTime;
            float a = Mathf.Lerp(1, 0, elapsedTime / coverFadeTime);
            titleCoverImage.color = new Color(0, 0, 0, a);
            yield return null;  
        }
        titleCoverImage.color = new Color(0, 0, 0, 0);
    }

    private IEnumerator FadeInTitle(TextMeshProUGUI text, float fadeTime)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float a = Mathf.Lerp(0, 1, elapsedTime / fadeTime);
            text.color = new Color(1, 1, 1, a);
            yield return null;
        }
        text.color = new Color(1, 1, 1, 1);
    }

    #region OnClick
    public void OnClickTitleToMainMenu()
    {
        //TODO 나중에 애니메이션 처리
        titleTouchArea.interactable = false;
        titlePanel.SetActive(false);
        mainmenuPanel.SetActive(true);
    }

    #endregion
}
