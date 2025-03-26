using System;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class UISettingPanel : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] private TMP_Dropdown dropDown_resolution;
    [SerializeField] private TMP_Dropdown dropDown_quality;
    [SerializeField] private TMP_Dropdown dropDown_fullscreen;
    
    public void OnSettingPanel()
    {
        //SoundManager.Instance.PlaySFX("");
        gameObject.SetActive(true);

    }

    public void InitializeSettingPanel()
    {
        LoadSettings();
    }

    public void OnClickApplySetting()
    {
        ApplySettings();
        SaveSettings();
    }

    private void ApplySettings()
    {
        SetResolution(dropDown_resolution.value);
        QualitySettings.SetQualityLevel(dropDown_quality.value);
    }
    private void SaveSettings()
    {
        PlayerPrefs.SetInt("Settings_Resolution", dropDown_resolution.value);
        PlayerPrefs.SetInt("Settings_Quality", dropDown_quality.value);
        PlayerPrefs.SetInt("Settings_FullScreen", dropDown_fullscreen.value);
        PlayerPrefs.Save();
    }
    private void LoadSettings()
    {
        dropDown_resolution.value = PlayerPrefs.GetInt("Settings_Resolution", 1);
        dropDown_quality.value = PlayerPrefs.GetInt("Settings_Quality", 1);
        dropDown_fullscreen.value = PlayerPrefs.GetInt("Settings_FullScreen", 1);
        ApplySettings();
    }

    private void SetResolution(int index)
    {
        int width = 0;
        int height = 0;
        switch (index)
        {
            case 0:
                width = 1280;
                height = 720;
                break;
            case 1:
                width = 1920;
                height = 1080;
                break;
            case 2:
                width = 1280;
                height = 720;
                break;
            case 3:
                width = 2560;
                height = 1440;
                break;
            default:
                width = 3840;
                height = 2160;
                break;
        }

        Screen.SetResolution(width, height, (dropDown_fullscreen.value == 1) ? true : false);
    }

}
