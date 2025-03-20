using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
{
    public void LoadScene(int sceneNum)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneNum);
    }
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
        SoundManager.Instance.PlayBGM($"BGM_{sceneName}");

    }
    public void LoadSceneAsync(int sceneNum)
    {
        Time.timeScale = 1f;
        StartCoroutine(CoLoadSceneWithLoading(SceneManager.LoadSceneAsync(sceneNum)));
    }
    public void LoadSceneAsync(string sceneName)
    {
        Time.timeScale = 1f;
        StartCoroutine(CoLoadSceneWithLoading(SceneManager.LoadSceneAsync(sceneName)));
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
