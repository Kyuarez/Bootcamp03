using TMPro;
using UnityEngine;

public class UIChat : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject chatLogPaenl;

    [Header("InputField")]
    [SerializeField] private TMP_InputField inputField;

    [Header("ChatLog")]
    [SerializeField] private RectTransform chatLogRect; //@tk 채팅 위치


    #region Onclick
    public void OnValueChanged()
    {

    }

    public void OnSelect()
    {

    }

    public void OnDeSelect()
    {

    }

    #endregion
}
