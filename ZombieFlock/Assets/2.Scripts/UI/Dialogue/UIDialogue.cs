using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDialogue : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Coroutine currentCoroutine;

    //@tk : 나중엔 음성과 싱크를 위해 Dialogue 클래스로 text, duration 정보 담아야 함.
    public void OnDialouge(Queue<string> dialogueBundle)
    {
        if(dialogueBundle == null || dialogueBundle.Count <= 0)
        {
            return;
        }

        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        panel.gameObject.SetActive(true);
        currentCoroutine = StartCoroutine(OnDialougeCo(dialogueBundle));
        
    }

    private IEnumerator OnDialougeCo(Queue<string> dialogueBundle)
    {
        
        yield return null;
    }
}
