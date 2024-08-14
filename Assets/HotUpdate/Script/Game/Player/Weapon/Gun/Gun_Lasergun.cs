using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Lasergun : Gun
{
    private GameObject effect;
    private LineRenderer laser;

    public int maxReflections = 3; // 将最大反弹次数修改为3
    public float maxLength = 100f;
    public float hitInterval = 0;
    public bool isCanHit = true;

    protected override void Awake()
    {
        base.Awake();
        laser = muzzlePoint.GetComponent<LineRenderer>();
        effect = transform.Find("Effect").gameObject;
    }

    public override void NoEnemyTarget()
    {
        StopFire();
    }

    public override void SingleFire()
    {
        if (!hasLockedEnemy) return;

        effect.SetActive(true);
        animator.SetTrigger("Fire");
        AudioManager.instance.PlayRandomSFXaudio(fireAudioData);

        laser.enabled = true;
        laser.positionCount = 1; // 初始化 LineRenderer 的位置数量为 1
        laser.SetPosition(0, muzzlePoint.position); // 设置起始位置为发射点

        int reflectionCount = 0; // 反射计数
        Vector2 currentPosition = muzzlePoint.position; // 从发射点开始

        int wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        for (int i = 0; i < maxReflections; i++)
        {
            // 先检测敌人
            RaycastHit2D[] hitsEnemies = Physics2D.RaycastAll(currentPosition, gunDirection, maxLength, enemyLayerMask);
            bool hitWall = false;

            foreach (var hitEnemy in hitsEnemies)
            {
                Debug.Log(hitEnemy.collider.gameObject.name + " 击中了");

                if (hitEnemy.collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy) && isCanHit)
                {
                    Vector3 hitPos= hitEnemy.collider.gameObject.transform.position;
                    Debug.Log("射线击中敌人");
                    Vector2 blackdiction = -gunDirection;

                    bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) < weaponInfo.baseCriticalRate;//计算暴击

                    float damage = isCriticalHit ? weaponInfo.damage*weaponInfo.criticalEffect : weaponInfo.damage;

                    if (isCriticalHit)
                    {
                        enemy.TakeDamageDiction((int)damage, -blackdiction.normalized, weaponInfo.backForce, hitPos);
                        DamageShowManager.instance.CreateRedDamage((int)damage, hitPos);
                    }
                    else
                    {
                        enemy.TakeDamageDiction((int)damage, -blackdiction.normalized, weaponInfo.backForce, hitPos);
                        DamageShowManager.instance.CreateDamage(weaponInfo.damage, hitPos);
                    }
                    player.AddHealth(damage*weaponInfo.vampire);//吸血

                }
            }

            if (isCanHit)
            {
                StartCoroutine(nameof(HitIntervalCoroutine));
            }

            // 然后检测墙壁
            RaycastHit2D hitWallDetection = Physics2D.Raycast(currentPosition, gunDirection, maxLength, wallLayerMask);
            Debug.DrawLine(currentPosition, currentPosition + gunDirection * maxLength, Color.green, 0.1f);
            if (hitWallDetection.collider != null)
            {
                Debug.Log(hitWallDetection.collider.gameObject.name + " 击中了");
                gunDirection = Vector2.Reflect(gunDirection, hitWallDetection.normal); // 计算反射方向
                currentPosition = hitWallDetection.point; // 将当前位置设置为墙壁的碰撞点

                hitWall = true;

                reflectionCount++;
                laser.positionCount = reflectionCount + 1; // 更新 LineRenderer 的位置数量
                laser.SetPosition(reflectionCount, currentPosition); // 设置反射点位置

                effect.transform.position = currentPosition;
            }
            // 如果没有击中墙壁，延伸激光
            if (!hitWall)
            {
                currentPosition += gunDirection * maxLength;

                reflectionCount++;
                laser.positionCount = reflectionCount + 1; // 更新 LineRenderer 的位置数量
                laser.SetPosition(reflectionCount, currentPosition); // 设置延伸点位置
                break; // 退出循环
            }
        }
    }

    public void StopFire()
    {
        laser.enabled = false;
        effect.SetActive(false);
    }

    IEnumerator HitIntervalCoroutine()
    {
        isCanHit = false;
        yield return new WaitForSeconds(hitInterval);
        isCanHit = true;
    }
}
