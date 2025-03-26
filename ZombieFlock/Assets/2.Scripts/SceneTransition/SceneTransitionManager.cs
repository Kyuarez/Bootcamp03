using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
{
    private UILoading uiLoading;

    protected override void Awake()
    {
        base.Awake();
        uiLoading = GetComponentInChildren<UILoading>();
        uiLoading.ResetUILoading();
    }
    public void LoadScene(string sceneName, Action action = null)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
        action?.Invoke();
        SoundManager.Instance.PlayBGM($"BGM_{sceneName}");
    }
    public void LoadSceneAsync(string sceneName, Action action = null)
    {
        Time.timeScale = 1f;
        StartCoroutine(CoLoadSceneWithLoading(sceneName, action));
        SoundManager.Instance.PlayBGM($"BGM_{sceneName}");
    }

    public void LoadSceneWithLoadingScene(string sceneName, Action action = null)
    {
        Time.timeScale = 1f; 
        StartCoroutine(LoadLoadingSceneAndNextScene(sceneName, action));
    }

    IEnumerator CoLoadSceneWithLoading(string sceneName, Action action = null)
    {
        //TODO
        yield return StartCoroutine(uiLoading.FadeCover(1f, 0f, 0.5f));
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
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
        yield return StartCoroutine(uiLoading.FadeCover(0f, 1f, 0.5f));
        uiLoading.ResetUILoading();
        action?.Invoke();
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
}
