using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Health")]
    [SerializeField] protected float maxHealth;
    private float health;  // 角色血量

    public virtual void TakeDamage(float damage)
    {
        if (health == 0) return;  // 先判断这个会消除下面的 bug
        health -= damage;

        if (health <= 0f)
        {
            health = 0f;
            Die();
        }
    }
    public virtual void Die()
    {
        gameObject.SetActive(false);
    }

}
