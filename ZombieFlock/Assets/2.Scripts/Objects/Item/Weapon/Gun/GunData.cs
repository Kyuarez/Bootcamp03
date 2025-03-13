using System;
using System.Collections;
using UnityEngine;

public class GunData : MonoBehaviour, IComparable
{
    public string Name;
    public GunType GunType;
    public AudioClip ShotSfx;
    public int magazineCount; //ÅºÃ¢ ¼ö
    public int bulletCount; //Åº¾Ë ¼ö

    public int CompareTo(object obj)
    {
        throw new NotImplementedException();
    }
}
