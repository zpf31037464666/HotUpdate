using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy_Bullet : MonoBehaviour
{
    public GameObject explosionPrefab;
    public AudioData hitAudioData;

    protected Vector3 hitPos;

    new protected Rigidbody2D rigidbody;
    protected WeaponInfo weaponInfo;
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
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        hitPos= other.transform.position;
        //打到敌人
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            Vector2 blackdiction = transform.position - hitPos;
            enemy.TakeDamageDiction(weaponInfo.damage, -blackdiction.normalized, weaponInfo.backForce, hitPos);
            DamageShowManager.instance.CreateDamage(weaponInfo.damage, transform.position);
            ObjectPool.Instance.PushObject(gameObject);
            CreateBooldEffect();
            AudioManager.instance.PlayRandomSFXaudio(hitAudioData);
        }
        if (other.gameObject.TryGetComponent(out Player player))
        {
            Vector2 blackdiction = transform.position - hitPos;
            player.TakeDamage(weaponInfo.damage);
            ObjectPool.Instance.PushObject(gameObject);
            CreateBooldEffect();
            AudioManager.instance.PlayRandomSFXaudio(hitAudioData);
        }
        //临时
        ObjectPool.Instance.PushObject(gameObject);
        CreateBooldEffect();
        AudioManager.instance.PlayRandomSFXaudio(hitAudioData);
    }
    protected virtual void CreateBooldEffect()
    {
        GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
        exp.transform.position = transform.position;
    }
}
