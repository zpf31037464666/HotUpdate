using Aliyun.OSS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Bullets & Shells")]
    [SerializeField] GameObject bulletPrefab;  // 子弹预制体
    [SerializeField] Transform muzzlePoint;  // 枪口位置
    [SerializeField] Transform shellPoint;  // 弹壳弹出位置

    [Header("Weapon stats")]
    [SerializeField] LayerMask enemyLayer;  // 敌人层
    [SerializeField] WeaponInfo weaponInfo; //数据层

    [Header("Aduio")]
    [SerializeField] AudioData fireAudioData;


    Vector2 gunDirection;
    SpriteRenderer spriteRenderer;
    Animator animator;

    Collider2D[] colliders;


    float time;

    Enemy lockedEnemy;
    bool hasLockedEnemy = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        LockEnemy();
        AutoFlip();
        time -= Time.deltaTime;
    }
    void LockEnemy()
    {
        if (hasLockedEnemy && (lockedEnemy == null || Vector2.Distance(lockedEnemy.transform.position, transform.position) > weaponInfo.fireRate||
            lockedEnemy.isDead ))
        {
            hasLockedEnemy = false;
            lockedEnemy = null;
        }

        if (!hasLockedEnemy)
        {
            colliders = Physics2D.OverlapCircleAll(transform.position,weaponInfo.fireRate, enemyLayer);
            if (colliders.Length > 0)
            {
                Collider2D randomEnemy = colliders[Random.Range(0, colliders.Length)];
                if (randomEnemy.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    lockedEnemy = enemy;
                    hasLockedEnemy = true;
                }
            }
        }

        if (hasLockedEnemy)
        {
            gunDirection = ((Vector2)(lockedEnemy.transform.position - transform.position)).normalized;
            transform.right = gunDirection;  // 朝向敌人
            if (time <= 0f)
            {
                SingleFire();
                time = weaponInfo.interval;
            }
        }
        else
        {
            gunDirection = Vector2.right;
            transform.right = gunDirection;  // 朝向右边
        }
    }
    void SingleFire()
    {
        if (hasLockedEnemy)
        {
            animator.SetTrigger("Fire");
            AudioManager.instance.PlayRandomSFXaudio(fireAudioData);
            GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
            bullet.transform.position = muzzlePoint.position;

            bullet.GetComponent<Bullet>().SetBulletInfo(weaponInfo);
            bullet.GetComponent<Bullet>().SetDiction(gunDirection);
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

    public void AddDamage(int damage)
    {
        weaponInfo.damage += damage;
    }

    public void AddFireSpeed(float precent)
    {
        weaponInfo.interval*=(1-precent);
    }
    public void AddCriticalRota(float precent)
    {
        weaponInfo.baseCriticalRate+=precent;
    }
    public void AddCriticalEffect(float precent)
    {
        weaponInfo.criticalEffect+=precent;
    }

}
