using UnityEngine;

public class UIMenuPanel : MonoBehaviour
{
    public void OnUIMenuPanel()
    {
        gameObject.SetActive(true);
    }

    public void OffUIMenuPanel()
    {
        gameObject.SetActive(false);
    }
}
