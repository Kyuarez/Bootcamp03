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
    public int magazineCount; //źâ ��
    public int bulletCount; //ź�� ��

    [Header("Shot Data")]
    public int gunDamage;
    public float gunMaxRange;
    public float shotDelay;
}
