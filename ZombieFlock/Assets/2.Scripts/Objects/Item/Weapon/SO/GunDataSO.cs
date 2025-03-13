using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName ="Item/Weapon", fileName ="Weapon_")]
public class GunDataSO : ScriptableObject
{
    [Header("Base Data")]
    public string CodeName;
    public GunType GunType;
    public AudioClip ShotSfx;
    public int magazineCount; //ÅºÃ¢ ¼ö
    public int bulletCount; //Åº¾Ë ¼ö

    [Header("Shot Data")]
    public int gunDamage;
    public float gunMaxRange;
    public float shotDelay;
}
