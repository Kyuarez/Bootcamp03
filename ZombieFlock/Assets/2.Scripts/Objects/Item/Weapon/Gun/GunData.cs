using System;
using System.Collections;
using UnityEngine;

public class GunData : MonoBehaviour, IComparable
{
    public string Name;
    public GunType GunType;
    public AudioClip ShotSfx;
    public int magazineCount; //źâ ��
    public int bulletCount; //ź�� ��

    public int CompareTo(object obj)
    {
        throw new NotImplementedException();
    }
}
