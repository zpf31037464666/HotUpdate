using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]  private float maxHealth;
    [SerializeField]  private float maxMP;//蓝量
    [SerializeField]  private float missPrecet;//闪避率
    [Header("Music")]
    [SerializeField] AudioData hurtAudioData;

    private float health;
    private float currentMp;
    private float InvincibleTime = .5f;//无敌时间

    private int currentLevel = 0;
    private float currentExp = 0;
    private float requiteExp = 10;


    private bool isInvincible=false;
    private Coroutine invincibleTimeCoroutine;
    private CinemachineImpulseSource impulseSource;



    public static Action<Player> OnChangeHealthEvent;
    public static Action<Player> OnHurtEvent;
    public static Action<Player> OnChangeMpEvent;
    public static Action<Player> OnChangeExpEvent;
    public static Action<Player> OnChangeLevelEvent;
    public static Action OnPlayerDeathEvent;


    public float Health { get => health; set => health=value; }
    public float MaxHealth { get => maxHealth; set => maxHealth=value; }
    public float MaxMP { get => maxMP; set => maxMP=value; }
    public float CurrentMp { get => currentMp; set => currentMp=value; }
    public float CurrentExp { get => currentExp; set => currentExp=value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel=value; }
    public float RequiteExp { get => requiteExp; set => requiteExp=value; }

    private void Start()
    {
        health=maxHealth;
        currentMp=maxMP;

        impulseSource=GetComponent<CinemachineImpulseSource>();
        CameraShake.instance.SetFollowPlayer(transform);

    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Test mp andd Exp");
            AddExp(2);
            UseMp(2);

        }
    }
    public virtual bool IsUseMp(float mp)=>currentMp>=mp;
    public virtual void UseMp(float mp)
    {
        currentMp-=mp;
        if(currentMp <= 0) currentMp = 0;
        OnChangeMpEvent?.Invoke(this);
    }
    public virtual void AddExp(float exp)
    {
        currentExp+=exp;
        OnChangeExpEvent?.Invoke(this);
        if (currentExp >= requiteExp)
        {
            Upgrade();
        }
    }
    public virtual void Upgrade()//玩家升级
    {
        currentLevel++;
        currentExp=0;
        requiteExp=requiteExp*1.2f;

        AddMp(maxMP);
        AddHealth(maxMP);

        OnChangeExpEvent?.Invoke(this);
        OnChangeLevelEvent?.Invoke(this);
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
    public virtual void AddHelath(int helath)
    {
        this.health += helath;
        if (health>=maxHealth)
        {
            health=maxHealth;
        }
        OnChangeHealthEvent?.Invoke(this);
    }
    public virtual void AddMp(float mp)
    { 
        currentMp+=mp;
        if(currentMp>=maxMP)
        {
            currentMp=maxMP;
        }
        OnChangeMpEvent?.Invoke(this);
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
        //    CameraShake.instance.ShakeCamera(InvincibleTime, 1, 1);
        CameraShake.instance.CamerShake(impulseSource);


        yield return new WaitForSeconds(InvincibleTime);
        isInvincible=false;
    }
}
