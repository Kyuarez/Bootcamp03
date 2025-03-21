using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
{
    public void LoadScene(int sceneNum, Action action = null)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneNum);
        action?.Invoke();
    }
    public void LoadScene(string sceneName, Action action = null)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
        action?.Invoke();

        SoundManager.Instance.PlayBGM($"BGM_{sceneName}");
    }
    public void LoadSceneAsync(int sceneNum, Action action = null)
    {
        Time.timeScale = 1f;
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneNum);
        StartCoroutine(CoLoadSceneWithLoading(ao));
        action?.Invoke();
    }
    public void LoadSceneAsync(string sceneName, Action action = null)
    {
        Time.timeScale = 1f;
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        StartCoroutine(CoLoadSceneWithLoading(ao));
        action?.Invoke();

        SoundManager.Instance.PlayBGM($"BGM_{sceneName}");
    }

    IEnumerator CoLoadSceneWithLoading(AsyncOperation ao)
    {
        ao.allowSceneActivation = false;
        //uiLoading.OnLoadingUI();
        //yield return new WaitForSeconds(loadingTime);
        ao.allowSceneActivation = true;
        //yield return new WaitForSeconds(loadingTime);
        //yield return StartCoroutine(uiLoading.FadeOut());
        //uiLoading.ResetLoadingUI();
        yield return null;
    }
}
