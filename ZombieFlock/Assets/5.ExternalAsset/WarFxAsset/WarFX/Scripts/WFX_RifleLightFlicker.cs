using System.Collections;
using UnityEngine;

//@tk : 해당 스크립트 가진 오브젝트 활성화 시, 반짝임 지속
[RequireComponent(typeof(Light))]
public class WFX_RifleLightFlicker : MonoBehaviour
{
    public float time = 0.05f;

    private float timer;

    private Coroutine coroutine;

    private void OnEnable()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        timer = time;
        coroutine = StartCoroutine(Flicker());
    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        timer = 0f;
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            GetComponent<Light>().enabled = !GetComponent<Light>().enabled;

            do
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            while (timer > 0);
            timer = time;
        }
    }
}
