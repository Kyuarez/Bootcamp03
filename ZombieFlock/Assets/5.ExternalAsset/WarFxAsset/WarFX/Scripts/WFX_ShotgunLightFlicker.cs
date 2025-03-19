using System.Collections;
using UnityEngine;

//@tk : 해당 스크립트 가진 오브젝트 활성화 시, 반짝임 지속
[RequireComponent(typeof(Light))]
public class WFX_ShotgunLightFlicker : MonoBehaviour
{
    private Coroutine coroutine;
    private float duration = 0.5f;

    private void OnEnable()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        coroutine = StartCoroutine(Flicker());
    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    IEnumerator Flicker()
    {
        float elapsedTime = Time.deltaTime;
        while (elapsedTime > duration)
        {

            yield return null;  
        }
    }
}
