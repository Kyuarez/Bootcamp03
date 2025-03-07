using TMPro;
using UnityEngine;

public class UIDev : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI frameText;

    private bool OnDevUI = false;
    private float deltaTime = 0.0f;

    private void Awake()
    {
        OnDevUI = Operator.Instance.IsDevMode;

        panel.SetActive(OnDevUI);
    }

    private void Update()
    {
        if (OnDevUI == false)
        {
            return;
        }

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float mesc = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        UpdateFrameText(mesc, fps); 
    }

    private void UpdateFrameText(float mesc, float fps)
    {
        frameText.text = string.Format("{0:0.0}ms ({1:0.}fps)", mesc, fps);
    }

}
