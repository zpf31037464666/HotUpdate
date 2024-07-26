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

    new protected Rigidbody2D rigidbody2D;  // 角色刚体
    protected Animator animator;  // 角色动画控制器
    protected SpriteRenderer spriteRenderer;  // 角色精灵渲染器

    protected float health;  // 角色血量
    protected Vector2 moveDirection;  // 角色移动方向

    new CircleCollider2D collider2D;
    WaitForSeconds waitForDie = new WaitForSeconds(1f);

    protected GameObject target;
    Ray ray;
    bool isBlocked;
    bool isTouchPlayer;

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
    protected virtual void FixedUpdate()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage);
            isTouchPlayer=true;
            Debug.Log(isTouchPlayer+"isTouchPlayer");
            Debug.Log("开始到玩家");
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage);
            isTouchPlayer=true;

            Debug.Log("停留到玩家");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            isTouchPlayer=false;
            Debug.Log("离开玩家");
            Debug.Log(isTouchPlayer+"isTouchPlayer");
        }
    }
    public virtual void FlipCharacter()
    {
        if (isTouchPlayer)
        {
            return;
        }
        if (moveDirection.x>0.1)
        {
            transform.localScale=new Vector3(-1,1, 1);
        }
        else if (moveDirection.x<-0.1)
        {
            transform.localScale=new Vector3(1, 1, 1);
        }

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
    }
    public virtual void Die()
    {
        gameObject.SetActive(false);
    }

    #region Move

    protected virtual void SimpleMove()
    {
        if (enemyIsDead || IsPathBlocked()||isTouchPlayer) return;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * 0.2f);
    }



}
