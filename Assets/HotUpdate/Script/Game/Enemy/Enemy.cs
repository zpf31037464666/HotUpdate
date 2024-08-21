using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected StatesBar headHealthBar;
    [SerializeField] protected float moveSpeed = 0.2f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected SpriteRenderer[] spriteRenderers; 

    [Header("Raycast")]
    [SerializeField] float raycastDistance = 0.2f;
    [SerializeField] LayerMask raycastLayerMask;

    public bool isDead = false;

    new protected Rigidbody2D rigidbody2D;  // 角色刚体
    protected Animator animator;  // 角色动画控制器

    protected float health;  // 角色血量
    protected Vector2 moveDirection;  // 角色移动方向

    Coroutine hurtCoroutine;
    protected new CircleCollider2D collider2D;

    protected GameObject target;
    protected Ray ray;
    protected bool isBlocked;

    protected bool isHurt;
    protected  float hurtPresistTime=.1f;

    protected LootSpawner lootSpawner;

    protected virtual void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        lootSpawner=GetComponent<LootSpawner>();
        collider2D = GetComponent<CircleCollider2D>();
        target = GameObject.FindGameObjectWithTag("Player");
    }
    protected virtual void Start()
    {
     
    }
    protected virtual void OnEnable()
    {
        health = maxHealth;
        headHealthBar.UpdateStates(health, maxHealth);
        rigidbody2D.drag = 100f;
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.material.SetFloat("_FlashAmount", 0);
        }
        collider2D.enabled = true;
        isDead = false;
        isHurt = false;

        WaveUI.OnRewardEvent += onRewardEvent;
        WaveUI.OnGameWin+=GameWin;

        Player.OnPlayerDeathEvent+=PlayerDeathEvent;
    }



    protected virtual void OnDisable()
    {
        headHealthBar.EmptyStates();
        StopAllCoroutines();

        WaveUI.OnRewardEvent -= onRewardEvent;
        WaveUI.OnGameWin-=GameWin;
        Player.OnPlayerDeathEvent-=PlayerDeathEvent;
    }
    protected virtual void onRewardEvent()
    {
        Die();
    }
    protected void GameWin()
    {
        Die();
    }
    private void PlayerDeathEvent()
    {
        Die();
    }

    public virtual  void Update()
    {
        SimpleMove();

        //if (Input.GetKeyDown(KeyCode.Space)) // 按下空格键时改变颜色
        //{
        //    Debug.Log("测试shader 随机颜色");
        //    var color = Color.green;
        //    foreach (var spriteRenderer in spriteRenderers)
        //    {
        //        spriteRenderer.material.SetColor("_Color", color); // 随机颜色
        //        spriteRenderer.material.SetColor("_FlashColor", color); // 随机闪烁颜色
        //        spriteRenderer.material.SetFloat("_FlashAmount", 0.2f);
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.P)) // 按下空格键时改变颜色
        //{
        //    Debug.Log("测试shader 还原颜色");
        //    var color = Color.white;
        //    foreach (var spriteRenderer in spriteRenderers)
        //    {
        //        spriteRenderer.material.SetColor("_Color", color); // 随机颜色
        //        spriteRenderer.material.SetColor("_FlashColor", color); // 随机闪烁颜色
        //        spriteRenderer.material.SetFloat("_FlashAmount", 0);
        //    }
        //}


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage);
        }
    }
    protected virtual void FlipCharacter()
    {
        if (moveDirection.x>0.1)
        {
            animator.transform.localScale=new Vector3(-1,1, 1);
        }
        else if (moveDirection.x<-0.1)
        {
            animator.transform.localScale=new Vector3(1, 1, 1);
        }

    }
    public virtual void TakeDamageDiction(int damage, Vector2 knockbackDirection, float knockbackForce, Vector2 hitPos)
    {
    //    Debug.Log("受到击退"+"Diction"+knockbackDirection+"force"+knockbackForce);
        rigidbody2D.velocity=knockbackDirection*knockbackForce;
        TakeDamage(damage);
    }
    public virtual void SetMaterial(Color color)
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.material.SetColor("_Color", color); // 随机颜色
            spriteRenderer.material.SetColor("_FlashColor", color); // 随机闪烁颜色
            spriteRenderer.material.SetFloat("_FlashAmount", 0.2f);
        }
    }
    public virtual void RestoreMaterial()
    {
        var color = Color.white;
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.material.SetColor("_Color", color); // 随机颜色
            spriteRenderer.material.SetColor("_FlashColor", color); // 随机闪烁颜色
            spriteRenderer.material.SetFloat("_FlashAmount", 0);
        }
    }
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
         headHealthBar.UpdateStates(health, maxHealth);

        if (health <= 0f)
        {
            health = 0f;

            lootSpawner.Spaw(transform.position);
            Debug.Log("敌人死亡临时 增加金币");
            TaskManager.instance.UpdateTaskState("CoinTask", 1);

            Die();
        }

        if (!gameObject.activeInHierarchy)
            return;
        if (hurtCoroutine != null)
        {
            StopCoroutine(hurtCoroutine);
        }
        hurtCoroutine=StartCoroutine(HurtCoroutine());
    }

    public virtual void SetMaxHealth(float health)
    {
        maxHealth = health;
        this.health=maxHealth;
    }
    public virtual void SetDamage(int amout)
    {
        damage = amout;
    }
    IEnumerator HurtCoroutine()
    {
        isHurt=true;
        HurtEffect();
        yield return new WaitForSeconds(hurtPresistTime);
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.material.SetFloat("_FlashAmount", 0);
        }

        isHurt = false;
        hurtCoroutine=null;
    }
    protected virtual void HurtEffect()
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.material.SetFloat("_FlashAmount", 1);
        }
        animator.SetTrigger("attack");//像是受击动作
    }
    protected virtual void Die()
    {
        isDead=true;
        headHealthBar.EmptyStates();
        StopAllCoroutines();
        ObjectPool.Instance.PushObject(gameObject);
    }
    #region Move
    protected virtual void SimpleMove()
    {
       if (isDead || IsPathBlocked()||isHurt||target==null) return;

        animator.SetBool("isMoving", true);
        moveDirection = (target.transform.position - transform.position).normalized;
         transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
       // rigidbody2D.velocity=moveDirection*moveSpeed;
         FlipCharacter();
    }
    protected bool IsPathBlocked()
    {
        
        ray = new Ray(transform.position, moveDirection);
        collider2D.enabled = false;
        isBlocked = Physics2D.Raycast(ray.origin, ray.direction, raycastDistance, raycastLayerMask);
        collider2D.enabled = true;
        return isBlocked;
    }
    #endregion
}
