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
        // 检查 target 是否为 null
        if (target == null)
        {
            // 在这里可以选择执行一些其他逻辑，例如寻找新的目标
            return; // 如果 target 为 null，就直接返回，不执行后续代码
        }

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
        if (target==null) return;

        Vector2 diction=target.transform.position-transform.position;
        var clone=Instantiate(buttle.gameObject);
        clone.transform.position=firePos.position;

        Enemy_Bullet enemy_Bullet= clone.GetComponent<Enemy_Bullet>();
        enemy_Bullet.SetBulletInfo(weaponInfo);
        enemy_Bullet.SetDiction(diction.normalized);
    }


}
