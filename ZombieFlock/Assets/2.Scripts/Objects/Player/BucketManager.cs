using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class BucketManager : MonoBehaviour
{
    [SerializeField] private Transform bucketTransform;
    [SerializeField] private Transform equippedTransfrom;

    private Queue<GameObject> weaponQueue;

    public Queue<GameObject> WeaponQueue
    {
        get { return weaponQueue; }
    }

    public Gun CurrentWeapon
    {
        get
        {
            if(equippedTransfrom.childCount == 0)
            {
                return null;
            }

            GameObject weapon = equippedTransfrom.GetChild(0).gameObject;
            Gun gun = weapon.GetComponent<Gun>();
            return gun;
        }
    }

    //@tk : (25.03.07) 일단, 총기다 로드. 나중에는 먹은 총기만 사용하도록
    public void InitBucket()
    {
        weaponQueue = new Queue<GameObject>();
    }
    
    public void EquippedWeapon()
    {
        if(equippedTransfrom.childCount == 0)
        {
            GameObject obj = weaponQueue.Dequeue();
            obj.transform.SetParent(equippedTransfrom);
            obj.transform.localPosition = Define.Rifle_Pos;
            obj.transform.localRotation = Quaternion.Euler(Define.Rifle_Rotate);
            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);    
            return;
        }

        if(equippedTransfrom.childCount > 0)
        {
            GameObject obj = equippedTransfrom.GetChild(0).gameObject;
            obj.SetActive(false);
            obj.transform.SetParent(bucketTransform);
            weaponQueue.Enqueue(obj);
        }

        GameObject weapon = weaponQueue.Dequeue();
        weapon.transform.SetParent(equippedTransfrom);
        weapon.transform.localPosition = Define.Rifle_Pos;
        weapon.transform.localRotation = Quaternion.Euler(Define.Rifle_Rotate);
        weapon.transform.localScale = Vector3.one;
        weapon.SetActive(true);
    }

    public void OnRegisterGun(GunData data)
    {
        GameObject weapon = Resources.Load<GameObject>(Define.RES_WEAPONS + "/" + data.CodeName);
        GameObject obj = Instantiate(weapon, bucketTransform);
        obj.GetComponent<Gun>().InitGunData(data);
        weaponQueue.Enqueue(obj);
        obj.SetActive(false);
    }

    public void OnHideWeapon()
    {
        if(CurrentWeapon == null)
        {
            return;
        }

        CurrentWeapon.gameObject.SetActive(false);
    }
    public void OnShowWeapon()
    {
        if (CurrentWeapon == null)
        {
            return;
        }

        CurrentWeapon.gameObject.SetActive(true);
    }
}
