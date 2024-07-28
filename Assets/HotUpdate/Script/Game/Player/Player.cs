using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Health")]
   [SerializeField]  private float maxHealth;
    private float health;
    public float InvincibleTime = .5f;
    private bool isInvincible=false;

    private Coroutine invincibleTimeCoroutine;

    public static Action<Player> OnChangeHealthEvent;

    public float Health { get => health; set => health=value; }
    public float MaxHealth { get => maxHealth; set => maxHealth=value; }


    private void Start()
    {
        health=maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (health == 0||isInvincible) return;  // 先判断这个会消除下面的 bug
        health -= damage;
        OnChangeHealthEvent?.Invoke(this);

        if (health <= 0f)
        {
            OnChangeHealthEvent?.Invoke(this);
            health = 0f;
            Die();
        }

        if (invincibleTimeCoroutine!=null)
        {
            StopCoroutine(invincibleTimeCoroutine);
            invincibleTimeCoroutine=StartCoroutine(InvincibleTimeCoroutine());
        }
        else
        {
            invincibleTimeCoroutine=StartCoroutine(InvincibleTimeCoroutine());
        }
    }

    public void AddHelath(int helath)
    {
        this.health += helath;
        if(health>=maxHealth)
        {
            health=maxHealth;
            OnChangeHealthEvent?.Invoke(this);
        }
        OnChangeHealthEvent?.Invoke(this);
    }
    public virtual void Die()
    {
       // gameObject.SetActive(false);
    }

    IEnumerator InvincibleTimeCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(InvincibleTime);
        isInvincible=false;
    }

}
