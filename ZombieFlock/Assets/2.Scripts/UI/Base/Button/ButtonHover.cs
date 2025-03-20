using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AnimationCurve OnEnterCurve;
    [SerializeField] private AnimationCurve OnExitCurve;
    [SerializeField, Range(1.0f, 2.0f)] private float targetRatio;

    private Vector3 originScale;
    private Vector3 targetScale;
    private bool isAnim = false;
    private float animDuration = 0.3f;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        originScale = transform.localScale;
        targetScale = transform.localScale * targetRatio;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(button.interactable == false)
        {
            return;
        }

        if (!isAnim) 
        {
            StopAllCoroutines();
            StartCoroutine(AnimateScale(targetScale, true));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable == false)
        {
            return;
        }

        if (!isAnim)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateScale(originScale, false));
        }
    }

    private IEnumerator AnimateScale(Vector3 target, bool isEnter)
    {
        isAnim = true;
        float elapsedTime = 0f;

        while (elapsedTime < animDuration)
        {
            float curveValue = (isEnter) ? OnEnterCurve.Evaluate(elapsedTime / animDuration) : OnExitCurve.Evaluate(elapsedTime / animDuration);
            Vector3 origin = (isEnter) ? originScale : targetScale;
            transform.localScale = Vector3.Lerp(origin, target, curveValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = target;
        isAnim = false;
    }
}
