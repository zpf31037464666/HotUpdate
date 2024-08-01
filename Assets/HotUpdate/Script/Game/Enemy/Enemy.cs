using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected StatesBar headHealthBar;
    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 0.2f;
    [SerializeField] protected int damage = 1;
    [Header("Raycast")]
    [SerializeField] float raycastDistance = 0.2f;
    [SerializeField] LayerMask raycastLayerMask;

    public bool isDead = false;

    new protected Rigidbody2D rigidbody2D;  // 角色刚体
    protected Animator animator;  // 角色动画控制器
    protected SpriteRenderer spriteRenderer;  // 角色精灵渲染器

    protected float health;  // 角色血量
    protected Vector2 moveDirection;  // 角色移动方向

    new CircleCollider2D collider2D;

    protected GameObject target;
    Ray ray;
    bool isBlocked;

    bool isHurt;
    float hurtPresistTime=.1f;

    protected virtual void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

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
        collider2D.enabled = true;
        rigidbody2D.drag = 100f;
        isDead = false;
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
        isHurt = false;

        WaveUI.OnRewardEvent+=onRewardEvent;
    }
    private void OnDisable()
    {
        WaveUI.OnRewardEvent-=onRewardEvent;

        headHealthBar.EmptyStates();
        StopAllCoroutines();
    }
    private void onRewardEvent()
    {
        Die();
    }

    private void Update()
    {
        SimpleMove();

        //Hurt();

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
    public virtual void FlipCharacter()
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
    Coroutine hurtCoroutine;
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        headHealthBar.UpdateStates(health, maxHealth);

        if (health <= 0f)
        {
            health = 0f;
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
    IEnumerator HurtCoroutine()
    {
        isHurt=true;
        HurtEffect();
        yield return new WaitForSeconds(hurtPresistTime);
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
        isHurt = false;
    }

    public virtual void HurtEffect()
    {
        spriteRenderer.material.SetFloat("_FlashAmount", 1);

    }
    public virtual void Die()
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * 0.2f);
    }
}
