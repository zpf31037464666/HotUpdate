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

        laser.enabled = true;
        effect.SetActive(true);
        animator.SetTrigger("Fire");
        AudioManager.instance.PlayRandomSFXaudio(fireAudioData);

        int reflectionCount = 0;
        Vector2 currentPosition = muzzlePoint.position;
        laser.positionCount = 1;
        laser.SetPosition(0, currentPosition);

        int wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        int combinedLayerMask = enemyLayerMask | wallLayerMask;

        while (reflectionCount < maxReflections)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, gunDirection, maxLength, combinedLayerMask);

            // 更新粒子特效位置
            effect.transform.position = hit.point;
            effect.transform.forward = -gunDirection;

            // 检查是否击中任何物体
            if (hit.collider != null)
            {
                // 碰到墙壁反弹
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    reflectionCount++;
                    laser.positionCount = reflectionCount + 1;
                    laser.SetPosition(reflectionCount, hit.point);
                    gunDirection = Vector2.Reflect(gunDirection, hit.normal);
                    currentPosition = hit.point + gunDirection * 0.01f; // 避免连续两次射线检测命中同一物体
                }
                // 检查是否击中敌人
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    GameObject other = hit.collider.gameObject;

                    if (other.TryGetComponent<Enemy>(out Enemy enemy) && isCanHit)
                    {
                        Debug.Log("射线击中敌人");
                        enemy.TakeDamage(weaponInfo.damage);
                        StartCoroutine(nameof(HitIntervalCoroutine));
                    }

                    // 在击中敌人后继续射击
                    currentPosition = hit.point + gunDirection * 0.01f; // 继续向前移动，避免连续命中
                    laser.positionCount = reflectionCount + 2; // 增加一个位置点
                    laser.SetPosition(reflectionCount + 1, currentPosition);
                }
            }
            else
            {
                // 如果没有击中任何物体，更新激光位置并退出循环
                currentPosition += maxLength * gunDirection;
                laser.positionCount = reflectionCount + 2;
                laser.SetPosition(reflectionCount + 1, currentPosition);
                break;
            }
        }

        // 确保激光的最后位置被更新
        laser.SetPosition(laser.positionCount - 1, currentPosition);
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
