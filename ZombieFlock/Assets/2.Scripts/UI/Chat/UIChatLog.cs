using TMPro;
using UnityEngine;

public class UIChatLog : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private TextMeshProUGUI chatText;

    public void UpdateChatLog(TKPacketChat packet)
    {
        string sendTime = string.Format("{0: HH:mm}", packet.SendTime);
        string nameColor = GetNameColorCode(packet.UserID);

        chatText.text = $"<color=#808080>[{sendTime}]</color> <color={nameColor}>{packet.NickName}</color> : {packet.Message}";
    }

    public void SetChatLogRect(float width, float height)
    {
        rect.localScale = new Vector3(width, height);
    }

    public string GetNameColorCode(string userID) 
    {
        if(userID == "1") //Server
        {
            return "#FFA07A";    
        }

        if(userID == ClientPacketManager.Instance.UserID)
        {
            return "#00CED1";
        }

        return "#32CD32";
    }
}
