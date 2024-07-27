using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected float maxHealth;
    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 0.2f;
    [SerializeField] protected float damage = 1f;
    [Header("Raycast")]
    [SerializeField] float raycastDistance = 0.2f;
    [SerializeField] LayerMask raycastLayerMask;
    [SerializeField] protected bool enemyIsDead = false;

    public float backForce;

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

    Coroutine hurtCoroutine;

    protected virtual void Awake()
    {

        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        collider2D = GetComponent<CircleCollider2D>();
        target = GameObject.FindGameObjectWithTag("Player");
    }
    protected virtual void OnEnable()
    {
        health = maxHealth;
        collider2D.enabled = true;
        rigidbody2D.drag = 100f;
        enemyIsDead = false;
    }
    private void Update()
    {
        SimpleMove();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            Debug.Log("碰到玩家");

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
            transform.localScale=new Vector3(-1,1, 1);
        }
        else if (moveDirection.x<-0.1)
        {
            transform.localScale=new Vector3(1, 1, 1);
        }

    }
    public virtual void TakeDamageDiction(float damage, Vector2 knockbackDirection, float knockbackForce, Vector2 hitPos)
    {
        Debug.Log("受到击退"+"Diction"+knockbackDirection+"force"+knockbackForce);

        rigidbody2D.velocity=knockbackDirection*knockbackForce;
        TakeDamage(damage);
    }
    public virtual void TakeDamage(float damage)
    {
        if (health == 0) return;  // 先判断这个会消除下面的 bug
        health -= damage;
        if (health <= 0f)
        {
            health = 0f;
            Die();
        }

        if (hurtCoroutine!=null)
        {
            StopCoroutine(hurtCoroutine);
            hurtCoroutine=StartCoroutine(HurtCoroutine());
        }
        else
        {
            hurtCoroutine=StartCoroutine(HurtCoroutine());
        }
    }
    public virtual void Die()
    {
        gameObject.SetActive(false);
    }
    #region Move
    protected virtual void SimpleMove()
    {
        if (enemyIsDead || IsPathBlocked()||isHurt) return;
         moveDirection = (target.transform.position - transform.position).normalized;
         transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
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

   public virtual IEnumerator HurtCoroutine()
    {
        isHurt = true;
        yield return new WaitForSeconds(0);
        isHurt=false;

        hurtCoroutine=null;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * 0.2f);
    }



}
