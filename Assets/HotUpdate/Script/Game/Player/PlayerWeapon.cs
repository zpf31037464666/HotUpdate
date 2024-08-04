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
     //   Debug.Log("增加武器次数");
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

    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.Escape))
    //    {
    //        Debug.Log("Test Add Gun");
    //        AddGun(initGun);
    //        //AddWeaponDamage(2);
    //        //AddWeaponFireSpeed(.3f);
    //    }
    //}

}
