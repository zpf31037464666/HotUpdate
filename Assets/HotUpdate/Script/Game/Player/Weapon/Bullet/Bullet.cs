using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected float speed;
    protected int damage;
    protected int backForce;//击退力
    public GameObject explosionPrefab;

    protected Vector3 hitPos;
    
    new private Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDiction(Vector2 direction)
    {
        rigidbody.velocity = direction * speed;
    }
    public void SetBulletInfo(WeaponInfo weaponInfo)
    {
        speed=weaponInfo.speed;
        damage=weaponInfo.damage;
        backForce=weaponInfo.backForce;
    }
    public void AddBulletDamage(int damage)
    {
       this.damage+=damage;
    }
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        hitPos= other.transform.position;
        ObjectPool.Instance.PushObject(gameObject);
        CreateBooldEffect();

        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            Vector2 blackdiction =transform.position - hitPos;
            enemy.TakeDamageDiction(damage, -blackdiction.normalized, backForce, hitPos);
            //enemy.TakeDamage(damage);
        }

    }
    protected virtual void CreateBooldEffect()
    {
        GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
        exp.transform.position = transform.position;
    }


}
