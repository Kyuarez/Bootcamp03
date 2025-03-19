using System.Collections;
using TMPro;
using UnityEngine;

public class UIDialogue : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    public void OnDialouge()
    {
        panel.gameObject.SetActive(true);
        
    }



    private IEnumerator OnDialougeCo()
    {
        yield return null;
    }
}
