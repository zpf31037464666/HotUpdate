using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;
    public int backForce;//击退力
    public GameObject explosionPrefab;

    private Vector3 hitPos;
    new private Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetSpeed(Vector2 direction)
    {
        rigidbody.velocity = direction * speed;
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
    void CreateBooldEffect()
    {
        GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
        exp.transform.position = transform.position;

    }


}
