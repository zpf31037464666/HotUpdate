using Aliyun.OSS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public ReboundsBullet beboundBullet;


    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            var bullet = ObjectPool.Instance.GetObject(beboundBullet.gameObject);
            bullet.transform.localPosition = transform.position;
            //  bullet.GetComponent<Bullet>().SetPlayer(player);

            var weaponInfo = new WeaponInfo();
            weaponInfo.speed=20;
            var gunDirection = transform.right;
            bullet.GetComponent<Bullet>().SetBulletInfo(weaponInfo);
            bullet.GetComponent<Bullet>().SetDiction(gunDirection);
        }
    }

}
