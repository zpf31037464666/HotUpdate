using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet :Bullet
{
    public override void OnTriggerEnter2D(Collider2D other)
    {
        hitPos= other.transform.position;
        CreateBooldEffect();
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            Vector2 blackdiction = transform.position - hitPos;
            enemy.TakeDamageDiction(damage, -blackdiction.normalized, backForce, hitPos);
            //enemy.TakeDamage(damage);
        }
    }

}
 
