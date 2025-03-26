using System.Collections;
using UnityEngine;

namespace TKCamera
{
    public class CameraShake : MonoBehaviour
    {
        [Header("Explosive Shake")]
        [SerializeField] private float ex_Duration = 0.5f;
        [SerializeField] private float ex_Magnitude = 0.025f;

        //[Header("Continous Shake")]
        //[SerializeField] private float co_Duration;
        //[SerializeField] private float co_Magnitude;

        //@tk : Camera Rag
        private Transform RagTransform;
        private Coroutine explosiveShakeCo;
        private Coroutine recoilShakeCo;
        //private Coroutine continousShakeCo;

        private void Awake()
        {
            RagTransform = (transform.parent != null) ? transform.parent.transform : transform;
        }

        //@tk : Granade
        public void ExplosiveCameraShake()
        {
            if(explosiveShakeCo != null)
            {
                StopCoroutine(explosiveShakeCo);
                explosiveShakeCo = null;
            }

            StartCoroutine(ExplosiveCameraShakeCo());
        }

        public void RecoilCameraShake(float duration, float magnitude)
        {

            if (recoilShakeCo != null)
            {
                StopCoroutine(recoilShakeCo);
                recoilShakeCo = null;
            }

            StartCoroutine(RecoilCameraShakeCo(duration, magnitude));
        }

        //public void ContinousCameraShake()
        //{
        //    if (continousShakeCo != null)
        //    {
        //        StopCoroutine(explosiveShakeCo);
        //        continousShakeCo = null;
        //    }

        //    StartCoroutine(ContinousCameraShakeCo());
        //}

        #region Coroutine
        private IEnumerator ExplosiveCameraShakeCo()
        {
            Vector3 originPos = RagTransform.localPosition;
            
            float elapsedTime = 0f;
            while(elapsedTime < ex_Duration)
            {
                float x = Random.Range(-1f, 1f) * ex_Magnitude;
                float y = Random.Range(-1f, 1f) * ex_Magnitude;

                RagTransform.localPosition = originPos + new Vector3(x, y, 0f);
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            RagTransform.localPosition = originPos;
            explosiveShakeCo = null;
        }

        //(25.03.18) @tk : test code, 나중엔 총 반동 실제 구현 
        private IEnumerator RecoilCameraShakeCo(float duration, float magnitude)
        {
            Vector3 originPos = RagTransform.localPosition;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                RagTransform.localPosition = originPos + new Vector3(x, y, 0f);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            RagTransform.localPosition = originPos;
            recoilShakeCo = null;
        }

        //private IEnumerator ContinousCameraShakeCo()
        //{
        //    yield return null;
        //}
        #endregion
    }
}
