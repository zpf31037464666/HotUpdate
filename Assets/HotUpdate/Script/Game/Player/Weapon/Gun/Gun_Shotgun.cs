using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Shotgun : Gun
{
    public int bulletNum = 3;
    public float bulletAngle = 15;
    public override void SingleFire()
    {
        if (hasLockedEnemy)
        {
            animator.SetTrigger("Fire");
            AudioManager.instance.PlayRandomSFXaudio(fireAudioData);
            StartCoroutine(FireCoroutine(bulletNum));
        }
    }
    IEnumerator FireCoroutine(int number)
    {
        int median = number / 2;
        for (int i = 0; i < number; i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
            bullet.transform.position = muzzlePoint.position;

            if (number % 2 == 1)
            {
                bullet.GetComponent<Bullet>().SetPlayer(player);
                bullet.GetComponent<Bullet>().SetBulletInfo(weaponInfo);
                bullet.GetComponent<Bullet>().SetDiction(Quaternion.AngleAxis(bulletAngle * (i - median), Vector3.forward) *gunDirection);
            }
            else
            {
                bullet.GetComponent<Bullet>().SetPlayer(player);
                bullet.GetComponent<Bullet>().SetBulletInfo(weaponInfo);
                bullet.GetComponent<Bullet>().SetDiction(Quaternion.AngleAxis(bulletAngle * (i - median), Vector3.forward) *gunDirection);
            }
        }

        //子弹壳
        //GameObject shell = ObjectPool.Instance.GetObject(shellPrefab);
        //shell.transform.position = shellPos.position;
        //shell.transform.rotation = shellPos.rotation;
        yield return null;
    }
}
