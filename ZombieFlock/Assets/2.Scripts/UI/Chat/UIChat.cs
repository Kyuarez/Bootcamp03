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

    private int chatCount = 0;

    //@tk : 이건 싱글, 멀티 구분
    public void OnOffChat(bool active)
    {
        if(panel.activeSelf != active)
        {
            panel.SetActive(active);
        }

        if(active == false)
        {
            ResetUIChat();
        }
    }

    public void ActivateInputField(bool active)
    {
        if(active == true)
        {
            inputField.ActivateInputField();
        }
        else
        {
            inputField.DeactivateInputField();
        }
    }

    public void UpdateChatLog(TKPacketChat packet)
    {
        chatCount++;

        //TODO 생성해서 -> chatLogRect에 하위로 넣기
        UIChatLog chatLog = Instantiate(chatLogObj).GetComponent<UIChatLog>();
        chatLog.transform.SetParent(chatLogRect);
        //chatLog.SetChatLogRect(600f, 50f); //TODO : 일단 때려 넣었는데, 이제 플랫폼 별 크기 대응하도록
        chatLog.UpdateChatLog(packet);
        chatLog.gameObject.SetActive(true);

        //위치 조정하기(일단...하드코딩...하자... 힘들다...)
        if(chatCount > (originHeight / chatLogHeight))
        {
            chatLogRect.sizeDelta = new Vector2(chatLogRect.sizeDelta.x, originHeight + (chatLogHeight * (chatCount - (originHeight / chatLogHeight))));
            chatLogRect.localPosition = new Vector3(chatLogRect.localPosition.x, originPosY + (chatLogHeight * (chatCount - (originHeight / chatLogHeight))), chatLogRect.localPosition.z);
        }
    }

    public void ResetUIChat()
    {
        if(chatLogRect.childCount > 0)
        {
            //TODO : 일단 이렇게 하는데 내일 시간되면 물어보자. (Pool vs 즉각 생성, 제거)
            chatLogRect.transform.DestroyImmediateAllChild();
        }

        chatLogRect.sizeDelta = new Vector2(chatLogRect.sizeDelta.x, originHeight);
        chatLogRect.localPosition = new Vector3(chatLogRect.localPosition.x, originPosY, chatLogRect.localPosition.z);
    }
    
    
    #region Onclick
    public void OnValueChanged()
    {
        //TODO : 글자수 넘어가면 제한 주기
    }

    public void OnEndEdit()
    {
        if(inputField.text == string.Empty || inputField.text == "")
        {
            return;
        }

        TKPacketChat packet = new TKPacketChat()
        {
            Message = inputField.text,
            SendTime = System.DateTime.Now,
            UserID = ClientPacketManager.Instance.UserID,
            NickName = "Unity",
        };

        bool isSuccess = ClientPacketManager.Instance.SendPacket(packet);

        inputField.text = string.Empty;
    }

    #endregion

    private readonly float originHeight = 300f;
    private readonly float originPosY = 0f;
    private readonly float chatLogHeight = 50f;
}
