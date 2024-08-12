using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Player : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]  private float maxHealth;
    [SerializeField]  private float maxMP;//蓝量
    [SerializeField]  private float speed;//速度
    [SerializeField]  private float critical;//暴击率
    [SerializeField]  private float dodge;//闪避率
    [SerializeField]  private float armor;//护甲
    [SerializeField]  private float vampire;//吸血率
    [SerializeField]  private float attackSpeed;//攻击速度

    private string playerName;

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
    private CinemachineImpulseSource impulseSource;//震动源头

    public static Action<Player> OnChangeHealthEvent;
    public static Action<Player> OnHurtEvent;
    public static Action<Player> OnChangeMpEvent;
    public static Action<Player> OnChangeExpEvent;
    public static Action<Player> OnChangeLevelEvent;
    public static Action OnPlayerDeathEvent;

    private PlayerWeapon playerWeapon;
    private PlayerUI playerUI;


    public float Health { get => health; set => health=value; }
    public float MaxHealth { get => maxHealth; set => maxHealth=value; }
    public float MaxMP { get => maxMP; set => maxMP=value; }
    public float CurrentMp { get => currentMp; set => currentMp=value; }
    public float CurrentExp { get => currentExp; set => currentExp=value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel=value; }
    public float RequiteExp { get => requiteExp; set => requiteExp=value; }
    public float Vampire { get => vampire; set => vampire=value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed=value; }
    public float Critical { get => critical; set => critical=value; }
    public float Speed { get => speed; set => speed=value; }
    private void Awake()
    {
        playerWeapon = GetComponent<PlayerWeapon>();
        playerUI=FindAnyObjectByType<PlayerUI>();
    }

    private void Start()
    {
        impulseSource=GetComponent<CinemachineImpulseSource>();
        CameraShake.instance.SetFollowPlayer(transform);
    }

    public void Init(PlayerItemData playerItemData)
    {
        maxHealth=playerItemData.Health;
        maxMP=playerItemData.MP;
        dodge=playerItemData.Dodge;
        armor=playerItemData.Armor;
        vampire=playerItemData.Vampire;
        attackSpeed=playerItemData.AttackSpeed;
        speed=playerItemData.Speed;
        critical=playerItemData.Critical;
        playerName=playerItemData.Name;

        health=maxHealth;
        currentMp=maxMP;

        OnChangeHealthEvent?.Invoke(this);
        OnChangeMpEvent?.Invoke(this);

        playerUI.SetNameText(playerItemData.Name);
        //加载图片
        Addressables.LoadAssetAsync<Sprite>(playerItemData.IconPath).Completed+=(handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                playerUI.SetHeadImage(handle.Result);
            }
        };

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
        bool isMiss = UnityEngine.Random.Range(0f, 1f)<dodge;
        if (isMiss)
        {
            DamageShowManager.instance.CreateDamage("Miss", transform.position);
            Invincble();
        }
        else
        {
            damage*=(1-armor);//护甲减伤
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
    public virtual void AddAttackSpeed(float percent)
    {
        attackSpeed*=percent;
        playerWeapon.AddWeaponFireSpeed(attackSpeed);
    }
    public virtual void DecreateAttackSpeed(float percent)
    {
        attackSpeed/=percent;
        playerWeapon.DecreateFireSpeed(attackSpeed);
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
        //  CameraShake.instance.ShakeCamera(InvincibleTime, 1, 1);
        CameraShake.instance.CamerShake(impulseSource);
        yield return new WaitForSeconds(InvincibleTime);
        isInvincible=false;
    }
}
