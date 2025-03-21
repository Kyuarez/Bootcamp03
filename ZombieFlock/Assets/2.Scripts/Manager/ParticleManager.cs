using System.Collections.Generic;
using UnityEngine;


public class ParticleManager : MonoSingleton<ParticleManager>
{

    protected override void Awake()
    {
        base.Awake();
    }

    public void PlayFX(EffectType effectType, Vector3 position)
    {
        //Pool에 있는지 물어보기
        if (PoolManager.Instance.IsExistPool(effectType.ToString()) == true)
        {
            GameObject poolObj = PoolManager.Instance.SpawnObjectInWorld(effectType.ToString(), position);
            ParticleSystem particle = poolObj.GetComponent<ParticleSystem>();
            
            if(particle != null)
            {
                if(particle.isPlaying == true)
                {
                    particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }

            particle.Play();
            return;
        }
        
        ParticleSystem fx = Instantiate(fxDict[effectType], position, Quaternion.identity);
        fx.Play();
        Destroy(fx.gameObject, fx.main.duration);
    }

    //@TK 이거 데이터 관리 따로 빼야함. Scriptable Object 등으로 ...
    protected Dictionary<EffectType, ParticleSystem> fxDict = new Dictionary<EffectType, ParticleSystem>();
}

