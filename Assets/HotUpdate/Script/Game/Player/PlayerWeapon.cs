using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] GameObject initGun;
    [SerializeField] Transform gunTransformGroup;
    public  List<Gun> gunList;
    public float radius;

    public List<AutoRotationToPlayer> rotationObjectList=new List<AutoRotationToPlayer>();
    private void Start()
    {
        AddGun(initGun);
    }
    public void AddGun(GameObject weapon)
    {
        GameObject clone = Instantiate(weapon, gunTransformGroup,false);
        gunList.Add(clone.GetComponent<Gun>());
        rotationObjectList.Add(clone.GetComponent<AutoRotationToPlayer>());
        foreach (var item in rotationObjectList)
        {
            item.RestPos(gunTransformGroup);
        }
    }
    public void AddWeaponDamage(int damage)
    {
        foreach(var gun in gunList)
        {
            gun.AddDamage(damage);
        }
    }
    public void AddWeaponFireSpeed(float precent)
    {
        foreach (var gun in gunList)
        {
            gun.AddFireSpeed(precent);
        }
    }
    public void DecreateFireSpeed(float precent)
    {
        foreach (var gun in gunList)
        {
            gun.DecreateSpeed(precent);
        }
    }
    //暴击率
    public void AddWeaponCriticalRate(float number)
    {
        foreach (var gun in gunList)
        {
            gun.AddCriticalRota(number);
        }
    }
    //暴击效果
    public void AddWeaponCriticalEffect(float number)
    {
        foreach (var gun in gunList)
        {
            gun.AddCriticalEffect(number);
        }
    }
}
