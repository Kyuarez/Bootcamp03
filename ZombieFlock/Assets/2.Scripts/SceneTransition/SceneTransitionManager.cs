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
        StartCoroutine(CoLoadSceneWithLoading(ao, action));
    }
    public void LoadSceneAsync(string sceneName, Action action = null)
    {
        Time.timeScale = 1f;
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        StartCoroutine(CoLoadSceneWithLoading(ao, action));
        SoundManager.Instance.PlayBGM($"BGM_{sceneName}");
    }

    IEnumerator CoLoadSceneWithLoading(AsyncOperation ao, Action action = null)
    {
        //TODO
        //Loading UI Control
        while(ao.isDone == false)
        {
            yield return null;
        }

        action?.Invoke();
    }
}
