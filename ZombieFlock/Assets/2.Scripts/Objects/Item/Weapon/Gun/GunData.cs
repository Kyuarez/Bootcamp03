using System;
using UnityEngine;

[Serializable]
public class GunData : MonoBehaviour
{
    public string CodeName;
    public GunType GunType;
    public AudioClip ShotSfx;
    public int magazineMaxCount; //źâ ��
    public int bulletMaxCount; //ź�� ��

    public int gunDamage;
    public float gunMaxRange;
    public float shotDelay;

    private void OnEnable()
    {
        //TODO : Resource���� �˾Ƽ� GunData �ޱ�
        string fileName = gameObject.name.Replace(prefix, "Weapon_");
        GunDataSO dataSO = Resources.Load<GunDataSO>(Define.RES_SO_GUN + "/" + fileName);

        if(dataSO == null)
        {
            return;
        }

        InitGunData(dataSO);
    }

    public void InitGunData(GunDataSO dataSO)
    {
        this.CodeName = dataSO.CodeName;
        this.GunType = dataSO.GunType;
        this.ShotSfx = dataSO.ShotSfx;
        this.magazineMaxCount = dataSO.magazineCount;
        this.bulletMaxCount = dataSO.bulletCount;

        this.gunDamage = dataSO.gunDamage;
        this.gunMaxRange = dataSO.gunMaxRange;
        this.shotDelay = dataSO.shotDelay;
    }


    private readonly string prefix = "Pickup_";
}
