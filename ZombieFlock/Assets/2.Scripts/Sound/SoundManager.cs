using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    private Coroutine currentBGMCoroutine;

    protected override void Awake()
    {
        base.Awake();
        InitializeAudioClip();
    }

    private void InitializeAudioClip()
    {
        bgmClipDict.Clear();
        sfxClipDict.Clear();

        foreach (var bgm in bgmClipList)
        {
            if(bgmClipDict.ContainsKey(bgm.name) == true)
            {
                continue;
            }

            bgmClipDict.Add(bgm.name, bgm.clip);
        }
        foreach (var sfx in sfxClipList)
        {
            if (bgmClipDict.ContainsKey(sfx.name) == true)
            {
                continue;
            }

            sfxClipDict.Add(sfx.name, sfx.clip);
        }
    }

    public void PlayBGM(string name, float fadeDuration = 1.0f)
    {
        if(bgmClipDict.ContainsKey(name) == false)
        {
            return;
        }

        if(currentBGMCoroutine != null)
        {
            StopCoroutine(currentBGMCoroutine);
            currentBGMCoroutine = null;
        }

        StartCoroutine(FadeOutBGMCo(fadeDuration, () =>
        {
            bgmSource.clip = bgmClipDict[name]; 
            bgmSource.Play();
            currentBGMCoroutine = StartCoroutine(FadeInBGMCo(fadeDuration));
        }));
    }
    //@tk : 이거 UI 효과음 하면 좋을 듯
    public void PlaySFX(string name)
    {
        if (sfxClipDict.ContainsKey(name) == false)
        {
            return;
        }

        sfxSource.PlayOneShot(sfxClipDict[name]);
    }
    public void PlaySFX(string name, Vector3 position)
    {
        if (sfxClipDict.ContainsKey(name) == false)
        {
            return;
        }

        AudioSource.PlayClipAtPoint(sfxClipDict[name], position);
    }
    public void PauseBGM()
    {
        bgmSource.Stop();
    }
    public void PauseSFX()
    {
        sfxSource.Stop();
    }
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }

    #region Coroutine
    private IEnumerator FadeOutBGMCo(float duration, Action onFadeCompleted)
    {
        float startVolume = bgmSource.volume;

        for (float time = 0; time < duration; time++)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        bgmSource.volume = 0;
        onFadeCompleted?.Invoke();
    }
    private IEnumerator FadeInBGMCo(float duration)
    {
        float startVolume = 0;
        bgmSource.volume = 0;

        for (float time = 0; time < duration; time++)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 1f, time / duration);
            yield return null;
        }

        bgmSource.volume = 1f;
    }

    #endregion

    private Dictionary<string, AudioClip> bgmClipDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxClipDict = new Dictionary<string, AudioClip>();

    [Serializable]
    public struct NameAudioClip
    {
        public string name;
        public AudioClip clip;
    }

    public NameAudioClip[] bgmClipList;
    public NameAudioClip[] sfxClipList;
}
