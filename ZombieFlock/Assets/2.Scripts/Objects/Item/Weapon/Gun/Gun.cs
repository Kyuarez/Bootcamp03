using TKCamera;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private GunData gunData;

    private int currentBulletCount;
    private int currentMagazineCount;

    public Transform Pos_ShotFX;
    public ParticleSystem ShotFX;
    private GameObject FX_Light;
    
    public GunData CurrentGunData
    {
        get { return gunData; }
    }
    public string CurrentGunName
    {
        get { return gunData.CodeName; } 
    }

    public int CurrentBulletCount
    {
        get { return currentBulletCount; }
    }
    public int CurrentMagazineCount
    {
        get { return currentMagazineCount; }
    }
    public int MaxBulletCount
    {
        get { return gunData.bulletMaxCount; }
    }
    public int MaxMagazineCount
    {
        get { return gunData.magazineMaxCount; }
    }

    public void InitGunData(GunData data)
    {
        gunData = data;
        currentBulletCount = gunData.bulletMaxCount;
        currentMagazineCount = gunData.magazineMaxCount;

        FX_Light = transform.GetComponentInChildren<Light>().gameObject;
        FX_Light.SetActive(false);
    }       

    public void OnShot()
    {   
        if (currentBulletCount <= 0)
        {
            return;
        }

        currentBulletCount--;
        Operator.Instance.CameraShake.RecoilCameraShake(CurrentGunData.recoilDuration, CurrentGunData.recoilMagnitude);
        OnShotFX(true);
    }
    public bool OnReloading()
    {
        if(currentMagazineCount <= 0)
        {
            return false;
        }

        currentBulletCount = gunData.bulletMaxCount;
        currentMagazineCount--;
        return true;
    }

    public void OnShotFX(bool active)
    {
        if(active == true)
        {
            ShotFX?.Play();
            FX_Light.SetActive(active);
        }
        else
        {
            ShotFX?.Stop();
            FX_Light.SetActive(active);
        }
    }
}
