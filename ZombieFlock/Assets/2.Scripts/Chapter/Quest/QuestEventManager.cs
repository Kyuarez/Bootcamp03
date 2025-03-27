using System;
using UnityEngine;

//@tk : 퀘스트 이벤트 트리거 조건을 관리하는 매니저
//퀘스트 매니저 쪽에서 이벤트 넣고, 파라미터로 이벤트 값 넣으면 알아서 업데이트 하는 방식
public class QuestEventManager
{
    public static event Action<QuestEventType> OnEventTriggered;

    public static void TriggerEvent(QuestEventType eventType)
    {
        OnEventTriggered?.Invoke(eventType);
    }
}
