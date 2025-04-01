using TMPro;
using UnityEngine;

public class UIChatLog : MonoBehaviour
{
    private RectTransform rect;
    private TextMeshProUGUI chatText;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    //@tk 패킷으로 받기
    public void UpdateChatLog(/*여기에 패킷 넣기*/)
    {
        //TODO : 생성해서 값 넣기
    }

    public void SetChatLogRect(float width, float height)
    {
        rect.localScale = new Vector3(width, height);
    }
}
