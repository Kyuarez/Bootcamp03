using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIChat : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject chatLogPanel;

    [Header("InputField")]
    [SerializeField] private TMP_InputField inputField;

    [Header("ChatLog")]
    [SerializeField] private GameObject chatLogObj;
    [SerializeField] private RectTransform chatLogRect; //@tk 채팅 위치
    

    public void OnChatLog(/*여기에 패킷*/)
    {
        //TODO 생성해서 -> chatLogRect에 하위로 넣기
        UIChatLog chatLog = Instantiate(chatLogObj).GetComponent<UIChatLog>();
        chatLog.transform.SetParent(chatLogRect);
        chatLog.SetChatLogRect(600f, 50f); //TODO : 일단 때려 넣었는데, 이제 플랫폼 별 크기 대응하도록
        chatLog.UpdateChatLog(/*여기에 패킷*/);
        chatLog.gameObject.SetActive(true);

        //위치 조정하기
    }
    
    
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
