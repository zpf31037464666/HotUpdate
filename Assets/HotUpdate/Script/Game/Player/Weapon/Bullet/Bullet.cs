using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosionPrefab;
    public AudioData hitAudioData;

    protected Vector3 hitPos;
    
    new protected Rigidbody2D rigidbody;
    protected WeaponInfo weaponInfo;
    protected Player player;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    public void SetDiction(Vector2 direction)
    {
        rigidbody.velocity = direction * weaponInfo.speed;
    }
    public void SetBulletInfo(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;
    }
    public void SetPlayer(Player player)
    {
        this.player = player;
    }
    public void AddBulletDamage(int damage)
    {
        weaponInfo.damage+=damage;
    }
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
  
        ObjectPool.Instance.PushObject(gameObject);
        CreateBooldEffect();
        AudioManager.instance.PlayRandomSFXaudio(hitAudioData);

        hitPos= other.transform.position;
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            Vector2 blackdiction =transform.position - hitPos;

            bool isCriticalHit=UnityEngine. Random.Range(0f, 1f) < weaponInfo.baseCriticalRate;//计算暴击

            float damage = isCriticalHit ? weaponInfo.damage*weaponInfo.criticalEffect : weaponInfo.damage;

            if (isCriticalHit)
            {
                enemy.TakeDamageDiction((int)damage, -blackdiction.normalized, weaponInfo.backForce, hitPos);
                DamageShowManager.instance.CreateRedDamage((int)damage, transform.position);
            }
            else
            {
                enemy.TakeDamageDiction((int)damage, -blackdiction.normalized, weaponInfo.backForce, hitPos);
                DamageShowManager.instance.CreateDamage(weaponInfo.damage, transform.position);
            }
            player.AddHealth(damage*weaponInfo.vampire);//吸血
        }

    }
    protected virtual void CreateBooldEffect()
    {
        GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
        exp.transform.position = transform.position;
    }


}
