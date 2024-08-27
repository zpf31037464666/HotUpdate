using Aliyun.OSS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReboundsBullet : Bullet
{
    public int maxBounces = 3; // 最大反弹次数
    public bool isStayScene=false;
    private int bounceCount = 0; // 当前反弹次数

    private float stayTime = 0f; // 停留时间
    private bool isCollidingWithWall = false; // 是否碰到墙壁
    private void Update()
    {
        transform.position +=(Vector3) direction.normalized * weaponInfo.speed * Time.deltaTime;
    }
    public override void SetDiction(Vector2 direction)
    {
       this.direction=direction.normalized;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.instance.PlayRandomSFXaudio(hitAudioData);
        hitPos = other.transform.position;
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            Vector2 blackdiction = transform.position - hitPos;

            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) < weaponInfo.baseCriticalRate; // 计算暴击

            float damage = isCriticalHit ? weaponInfo.damage * weaponInfo.criticalEffect : weaponInfo.damage;

            if (isCriticalHit)
            {
                enemy.TakeDamageDiction((int)damage, -blackdiction.normalized, weaponInfo.backForce, hitPos);
                DamageShowManager.instance.CreateDamage((int)damage, transform.position, Color.red);
            }
            else
            {
                enemy.TakeDamageDiction((int)damage, -blackdiction.normalized, weaponInfo.backForce, hitPos);
                DamageShowManager.instance.CreateDamage(weaponInfo.damage, transform.position);
            }
            player.AddHealth(damage * weaponInfo.vampire); // 吸血
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 检查是否碰到墙壁
        if (collision.gameObject.CompareTag("Wall"))
        {
            isCollidingWithWall = true; // 开始与墙壁碰撞
            direction = Vector2.Reflect(direction, collision.contacts[0].normal).normalized;
            stayTime = 0f; // 重置停留时间
            bounceCount++; // 增加反弹次数

            if (bounceCount > maxBounces)
            {
                if(!isStayScene)//是否永久停留在场上
                {
                    ObjectPool.Instance.PushObject(gameObject);
                    CreateBooldEffect();
                    bounceCount = 0;
                }
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 继续检查是否与墙壁碰撞
        if (isCollidingWithWall && collision.gameObject.CompareTag("Wall"))
        {
            stayTime += Time.deltaTime; // 增加停留时间
            // 如果停留时间超过 1 秒，增加反弹次数
            if (stayTime >= 0.1f)
            {
                direction = -direction; // 反转方向
                stayTime = 0f; // 重置停留时间

                bounceCount++; // 增加反弹次数
                if (bounceCount >= maxBounces)
                {
                    if (!isStayScene)//是否永久停留在场上
                    {
                        ObjectPool.Instance.PushObject(gameObject);
                        CreateBooldEffect();
                        bounceCount = 0;
                    }
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 退出碰撞时重置状态
        if (collision.gameObject.CompareTag("Wall"))
        {
            isCollidingWithWall = false; // 不再与墙壁碰撞
            stayTime = 0f; // 重置停留时间
        }
    }
}
