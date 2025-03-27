using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
{
    private UILoading uiLoading;

    protected override void Awake()
    {
        base.Awake();
        uiLoading = GetComponentInChildren<UILoading>();
        uiLoading.ResetUILoading();
    }
    public void LoadSceneTitleAsync()
    {
        Time.timeScale = 1f;
        StartCoroutine(CoLoadSceneTitle());
        SoundManager.Instance.PlayBGM($"BGM_TitleScene");
        Cursor.lockState = CursorLockMode.None;
    }
    public void LoadSceneAsync(Chapter chapter, Action action = null)
    {
        Time.timeScale = 1f;
        StartCoroutine(CoLoadSceneWithLoading(chapter, action));
        SoundManager.Instance.PlayBGM($"BGM_{chapter.sceneName}");
    }

    IEnumerator CoLoadSceneWithLoading(Chapter chapter, Action action = null)
    {
        //@tk : 실제 씬 로드랑 별개로 로딩 씬 하기 (고정 값 2~3초)
        yield return StartCoroutine(uiLoading.FadeCover(0f, 1f, 0.5f));
        uiLoading.OnLoadingPage(chapter);
        yield return StartCoroutine(uiLoading.PreparedVideoCo());
        //sceneLogic
        AsyncOperation ao = SceneManager.LoadSceneAsync(chapter.sceneName);
        ao.allowSceneActivation = false;
        //Loading UI Control
        while(ao.isDone == false)
        {
            if(ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }

        //TODO : 스페이스 누르면 시작으로 변경
        action?.Invoke();
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(uiLoading.FadeCover(1f, 0f, 0.5f));
        uiLoading.ResetUILoading();
    }
    IEnumerator CoLoadSceneTitle(Action action = null)
    {
        //@tk : 실제 씬 로드랑 별개로 로딩 씬 하기 (고정 값 2~3초)
        yield return StartCoroutine(uiLoading.FadeCover(0f, 1f, 0.5f));
        uiLoading.OnLoadingPage();
        yield return StartCoroutine(uiLoading.PreparedVideoCo());
        //sceneLogic
        AsyncOperation ao = SceneManager.LoadSceneAsync("TitleScene");
        ao.allowSceneActivation = false;
        //Loading UI Control
        while (ao.isDone == false)
        {
            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }

        //TODO : 스페이스 누르면 시작으로 변경
        action?.Invoke();
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(uiLoading.FadeCover(1f, 0f, 0.5f));
        uiLoading.ResetUILoading();
    }


    #region NotUse
    public void LoadSceneWithLoadingScene(string sceneName, Action action = null)
    {
        Time.timeScale = 1f; 
        StartCoroutine(LoadLoadingSceneAndNextScene(sceneName, action));
    }

    public IEnumerator LoadLoadingSceneAndNextScene(string nextScene, Action action = null)
    {
        AsyncOperation sceneAO = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        sceneAO.allowSceneActivation = false;

        while (!sceneAO.isDone)
        {
            if (sceneAO.progress >= 0.9f)
            {
                sceneAO.allowSceneActivation = true;
            }
            yield return null;
        }
        SceneManager.UnloadSceneAsync("TitleScene");

        Slider loadingbar = GameObject.Find("@LoadingBar").GetComponent<Slider>();
        AsyncOperation nextSceneAO = SceneManager.LoadSceneAsync(nextScene);
        
        while (!nextSceneAO.isDone)
        {
            loadingbar.value = nextSceneAO.progress;
            yield return null;
        }
        SoundManager.Instance.PlayBGM($"BGM_{nextScene}");
        action?.Invoke();
        SceneManager.UnloadSceneAsync("LoadingScene");
        yield return null;
    }
    #endregion
}
