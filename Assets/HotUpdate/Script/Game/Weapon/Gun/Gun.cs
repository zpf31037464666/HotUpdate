using Aliyun.OSS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Bullets & Shells")]
    [SerializeField] GameObject bulletPrefab;  // 子弹预制体
  //  [SerializeField] GameObject shellPrefab;  // 弹壳预制体
    [SerializeField] Transform muzzlePoint;  // 枪口位置
    [SerializeField] Transform shellPoint;  // 弹壳弹出位置

    [Header("Weapon stats")]
    [SerializeField] LayerMask enemyLayer;  // 敌人层

    Vector2 gunDirection;
    SpriteRenderer spriteRenderer;
    Animator animator;

    Collider2D[] colliders;


    float damage=10;
    float gunRange=10;
    float fireRate = 10;
    float fireTimer=.2f;
    float time;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        AutoFollowenemy();
        AutoFlip();
        time -= Time.deltaTime;
    }

    void SingleFire()
    {
        animator.SetTrigger("Fire");
        GameObject bullet=ObjectPool.Instance.GetObject(bulletPrefab);
        bullet.transform.position=muzzlePoint.position;
        bullet.GetComponent<Bullet>().SetSpeed(gunDirection);

         //GameObject shell=  ObjectPool.Instance.GetObject(shellPrefab);
         //shell.transform.localPosition=shellPoint.position;
         //shell.transform.localRotation=shellPoint.rotation;
    }

    void AutoFollowenemy()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position,
            fireRate, enemyLayer);  // TODO: 优化，不要每帧都获取范围等参数
        if (colliders.Length == 0)
        {
            gunDirection = Vector2.right;
            transform.right = gunDirection;  // 朝向右边
            return;
        }

        Collider2D randomEnemy = colliders[Random.Range(0, colliders.Length)];
        if (randomEnemy.TryGetComponent<Enemy>(out Enemy enemy))
        {
            gunDirection = ((Vector2)(enemy.transform.position - transform.position)).normalized;
            transform.right = gunDirection;  // 朝向敌人
            if (time <= 0f)
            {
                SingleFire();
                time =fireTimer;
            }
        }
    }

    void AutoFlip()
    {
        spriteRenderer.flipY = gunDirection.x < 0f;
    }

    public void SetGunMaterial(Material material)
    {
        spriteRenderer.material = material;
    }

}
