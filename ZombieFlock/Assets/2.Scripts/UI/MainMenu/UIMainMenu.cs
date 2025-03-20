using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Button btn_newGame;
    [SerializeField] private Button btn_chapter;
    [SerializeField] private Button btn_multigame;
    [SerializeField] private Button btn_setting;
    [SerializeField] private Button btn_exit;


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
        btn_newGame.onClick.AddListener(OnClickOnNewGame);
        btn_chapter.onClick.AddListener(OnClickOnChapter);
        btn_multigame.onClick.AddListener(OnClickOnMultiGame);
        btn_setting.onClick.AddListener(OnClickOnSettings);
        btn_exit.onClick.AddListener(OnClickOnExit);

        ResetUITitle();
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM("BGM_TitleScene");

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

    #region Coroutine
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
    #endregion

    #region OnClick
    public void OnClickTitleToMainMenu()
    {
        //TODO 나중에 애니메이션 처리
        titleTouchArea.interactable = false;
        titlePanel.SetActive(false);
        mainmenuPanel.SetActive(true);
    }

    public void OnClickOnNewGame()
    {
        //TODO : SceneManager를 통해서 씬 이동
        SceneTransitionManager.Instance.LoadScene("Level0306");
    }

    public void OnClickOnChapter()
    {

    }

    public void OnClickOnMultiGame()    
    {

    }

    public void OnClickOnSettings()
    {

    }

    public void OnClickOnExit()
    {
        Application.Quit();
    }
    #endregion
}
