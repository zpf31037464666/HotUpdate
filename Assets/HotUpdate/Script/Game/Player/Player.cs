using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]  private float maxHealth;
    [SerializeField]  private float missPrecet;
    [Header("Music")]
    [SerializeField] AudioData hurtAudioData;

    private float health;
    public float InvincibleTime = .5f;//无敌时间
    private bool isInvincible=false;
    private Coroutine invincibleTimeCoroutine;

    public static Action<Player> OnChangeHealthEvent;
    public static Action<Player> OnHurtEvent;
    public static Action OnPlayerDeathEvent;

    public float Health { get => health; set => health=value; }
    public float MaxHealth { get => maxHealth; set => maxHealth=value; }


    private void Start()
    {
        health=maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (health == 0||isInvincible) return;  // 先判断这个会消除下面的 bug
        bool isMiss = UnityEngine.Random.Range(0f, 1f)<missPrecet;
        if (isMiss)
        {
            DamageShowManager.instance.CreateDamage("Miss", transform.position);
            Invincble();
        }
        else
        {
            health -= damage;

            AudioManager.instance.PlayRandomSFXaudio(hurtAudioData);

            OnChangeHealthEvent?.Invoke(this);
            OnHurtEvent?.Invoke(this);

            if (health <= 0f)
            {
                OnChangeHealthEvent?.Invoke(this);
                health = 0f;
                Die();
            }

            Invincble();
        }
    }

    public virtual void AddHealth(float amont)
    {
        health+=amont;
        if (health>=maxHealth) health=maxHealth;
        OnChangeHealthEvent?.Invoke(this);
    }
    private void Invincble()
    {
        if (!gameObject.activeInHierarchy)
            return;
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
        StopAllCoroutines();
        gameObject.SetActive(false);
        OnPlayerDeathEvent?.Invoke();
    }
    IEnumerator InvincibleTimeCoroutine()
    {
        isInvincible = true;
        //  CameraRecoil.instance.Shake(InvincibleTime, .1f);
        CameraShake.instance.ShakeCamera(InvincibleTime, 1, 1);
        yield return new WaitForSeconds(InvincibleTime);
        isInvincible=false;
    }
}
