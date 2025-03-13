using UnityEngine;

public class Gun : MonoBehaviour
{
    private GunData gunData;

    private int currentBulletCount;
    private int currentMagazineCount;

    public Transform Pos_ShotFX;
    public ParticleSystem ShotFX;
    
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
    }

    public void OnShot()
    {
        if (currentBulletCount <= 0)
        {
            return;
        }

        currentBulletCount--;
        ShotFX.Play();
    }
    public void OnReloading()
    {
        if(currentMagazineCount <= 0)
        {
            return;
        }

        currentBulletCount = gunData.bulletMaxCount;
        currentMagazineCount--;
    }
}
