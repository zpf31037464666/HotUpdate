using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] Gun initGun;
    [SerializeField] List<Transform> gunTransformList;
    [SerializeField] List<Gun> gunList;

    private void Start()
    {
        AddGun(initGun);
    }

    int gunNumber = 0;

    public void AddGun(Gun weapon)
    {
        Debug.Log("增加武器次数");

   
        GameObject clone=  Instantiate(weapon.gameObject, gunTransformList[gunNumber]);
        gunList.Add(clone.GetComponent<Gun>());
        gunNumber=(gunNumber+1)%gunTransformList.Count;
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
    private void Update()
    {
        //if(Input.GetKeyUp(KeyCode.Escape))
        //{
        //    Debug.Log("Test Add Gun");
        //     AddGun(initGun);
        //     AddWeaponDamage(2);
        //    AddWeaponFireSpeed(.3f);
        //}
    }

}
