using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Remote : Enemy
{
    [SerializeField] Enemy_Bullet buttle;
    [SerializeField] Transform firePos;
    [SerializeField] private WeaponInfo weaponInfo;

    bool isFire;

    float time;
    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(target.transform.position, transform.position)<weaponInfo.fireRate&&time<=0f&&target!=null)
        {
            isFire=true;
            time = weaponInfo.interval;
            Fire();
        }
        time-= Time.deltaTime;
    }
    void Fire()
    {
        Vector2 diction=target.transform.position-transform.position;
        var clone=Instantiate(buttle.gameObject);
        clone.transform.position=firePos.position;

        Enemy_Bullet enemy_Bullet= clone.GetComponent<Enemy_Bullet>();
        enemy_Bullet.SetBulletInfo(weaponInfo);
        enemy_Bullet.SetDiction(diction.normalized);
    }


}
